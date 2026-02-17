using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

using Airflights.Data;
using Airflights.Services;

namespace Airflights
{
    public class AirflightsApp
    {
        private WebApplication _app;
        public AirflightsApp(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            builder = ConfigureServices(builder);
            _app = builder.Build();
            ConfigurePipeline();
        }
        
        private WebApplicationBuilder ConfigureServices(WebApplicationBuilder builder)
        {
            // Настройка БД
            var appFolder = AppContext.BaseDirectory;
            var dbPath = Path.Combine(appFolder, "airflits.db");

            Console.WriteLine($"📁 База данных будет создана: {dbPath}");
            
            // Регистрация SQLite с автоматическим созданием файла
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));
            // builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("TodoList"));

            // Регистрация сервиса инициализации БД
            builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IDictionaryService, DictionaryService>();
            builder.Services.AddScoped<IAircraftsService, AircraftsService>();
            builder.Services.AddScoped<IFlightsService, FlightsService>();
            builder.Services.AddScoped<IFlightsShedulerService, FlightsShedulerService>();
            
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/login"; // Страница входа
                options.LogoutPath = "/logout";
                options.AccessDeniedPath = "/access-denied";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
            });
            builder.Services.AddAuthorization();
            
            builder.Services.AddHostedService<PeriodicTaskService>();
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Swagger/OpenAPI
            builder.Services.AddOpenApiDocument(config =>
            {
                config.DocumentName = "AirflitsAPI";
                config.Title = "AirflitsAPI v1";
                config.Version = "v1";
            });

            // Логирование
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            return builder;
        }
        
        private void ConfigurePipeline()
        {
            // Инициализация БД
            InitializeDatabase().GetAwaiter().GetResult();
            // Настройка middleware
            if (_app.Environment.IsDevelopment())
            {
                _app.UseDeveloperExceptionPage();
                _app.UseOpenApi();
                _app.UseSwaggerUi(config =>
                {
                    config.DocumentTitle = "AirflitsAPI";
                    config.Path = "/swagger";
                    config.DocumentPath = "/swagger/{documentName}/swagger.json";
                    config.DocExpansion = "list";
                });
            }
            _app.UseHttpsRedirection();
            _app.UseAuthentication(); // Должен быть перед UseAuthorization
            _app.UseAuthorization();
            _app.MapControllers();
            _app.MapRazorPages();

        }
        private async Task InitializeDatabase()
        {
            using var scope = _app.Services.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            var appFolder = AppContext.BaseDirectory;
            var dbPath = Path.Combine(appFolder, "airflits.db");
            
            await initializer.InitializeAsync(dbPath);
        }
        public void Run()
        {
            try
            {
                Console.WriteLine("🚀 Запуск Todo API...");
                Console.WriteLine("🌐 Swagger UI: http://localhost:5000/swagger");
                Console.WriteLine("⏹️  Для остановки нажмите Ctrl+C\n");
                
                _app.Run("http://127.0.0.1:5000");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при запуске приложения: {ex.Message}");
            }
        }
    }
}