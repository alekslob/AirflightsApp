using Microsoft.EntityFrameworkCore;
using Airflights.Models;
using Airflights.Utils;

namespace Airflights.Data
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync(string dbPath);
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(
            ApplicationDbContext context, 
            ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync(string dbPath)
        {
            try
            {
                _logger.LogInformation("🔍 Проверка базы данных...");
                
                // Проверяем, существует ли файл БД
                if (!File.Exists(dbPath))
                {
                    _logger.LogInformation("📁 Файл базы данных не найден. Создаем...");
                }
                else
                {
                    _logger.LogInformation("✅ Файл базы данных найден.");
                }
                // Создаем таблицы, если их нет
                _logger.LogInformation("🛠️ Создание структуры базы данных...");
                await _context.Database.EnsureCreatedAsync();
                _logger.LogInformation("✅ Структура базы данных проверена/создана.");
                
                
                await SeedTestDataAsync();
                
                _logger.LogInformation("🎉 Инициализация базы данных завершена успешно!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Ошибка при инициализации базы данных");
                throw;
            }
        }
        private async Task SeedTestDataUser()
        {
            string baseLogin = "admin";
            string basePwd = "111";
            var system = await _context.Users.FirstOrDefaultAsync(u => u.Login == baseLogin); 
            if (system != null) return;
            _logger.LogInformation("➕ Добавляем базовый аккаунт");
            var user = new User
            {
                Name = "Админ",
                Login = baseLogin,
                Hash = PasswordHelper.HashPassword(basePwd),
                Role = UserRoles.Admin,
                CreatedAt = DateTime.Now

            };
            await _context.Users.AddRangeAsync(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Тестовые данные добавлены.");
        }
        private async Task SeedTestDataUserManager()
        {
            string baseLogin = "man";
            string basePwd = "111";
            var system = await _context.Users.FirstOrDefaultAsync(u => u.Login == baseLogin); 
            if (system != null) return;
            _logger.LogInformation("➕ Добавляем базовый аккаунт");
            var user = new User
            {
                Name = "Манагер",
                Login = baseLogin,
                Hash = PasswordHelper.HashPassword(basePwd),
                Role = UserRoles.Manager,
                CreatedAt = DateTime.Now

            };
            await _context.Users.AddRangeAsync(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Тестовые данные добавлены.");
        }
        private async Task SeedTestDataCities()
        {
            var count = await _context.Cities.CountAsync();
            _logger.LogInformation("📊 В базе {TodoCount} записей.", count);
            if (count>0) return;
            _logger.LogInformation("➕ Добавляем тестовые данные...");
            var testCities = new List<City>
            {
                new() { Name="Москва", IadaCode = "MOW"},
                new() { Name="Санкт-Петербург", IadaCode = "LED"},
                new() { Name="Новсибирск", IadaCode = "OVB"},
                new() { Name="Екатеринбург", IadaCode = "SVX"},
                new() { Name="Казань", IadaCode = "KZN"}
            };
            
            await _context.Cities.AddRangeAsync(testCities);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Тестовые данные добавлены.");

        }

        private async Task SeedTestDataAirports()
        {
            var count = await _context.Airports.CountAsync();
            _logger.LogInformation("📊 В базе {TodoCount} записей.", count);
            if (count>0) return;
            _logger.LogInformation("➕ Добавляем тестовые данные...");
            var testAirports = new List<Airport>
            {
                new() { Name="Шереметьево", IadaCode = "SVO", CityId=1},
                new() { Name="Домодедово", IadaCode = "DME", CityId=1},
                new() { Name="Внуково", IadaCode = "VKO", CityId=1},
                new() { Name="Жуковский", IadaCode = "ZIA", CityId=1},
                new() { Name="Пулково", IadaCode = "LED", CityId=2},
                new() { Name="Толмачёво", IadaCode = "OVB", CityId=3},
                new() { Name="Кольцово", IadaCode = "SVX", CityId=4},
                new() { Name="Казань", IadaCode = "KZN", CityId=5}
            };
            
            await _context.Airports.AddRangeAsync(testAirports);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Тестовые данные добавлены.");

        }
        
        private async Task SeedTestDataAircraftModels()
        {
            var count = await _context.AircraftModels.CountAsync();
            _logger.LogInformation("📊 В базе {TodoCount} записей.", count);
            if (count > 0) return;
            _logger.LogInformation("➕ Добавляем тестовые данные...");
            var test = new List<AircraftModel>
            {
                new() {Name="Airbus A320-200", TotalSeats=180, SeatsPerRow=4, Rows=45},
                new() {Name="Boeing 737-800", TotalSeats=188, SeatsPerRow=4, Rows=47},
                new() {Name="Boeing 777-300ER", TotalSeats=396, SeatsPerRow=6, Rows=66},
                new() {Name="Airbus A350-900", TotalSeats=318, SeatsPerRow=6, Rows=53},
                new() {Name="Sukhoi Superjet 100", TotalSeats=98, SeatsPerRow=4, Rows=24}
            };
            
            await _context.AircraftModels.AddRangeAsync(test);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Тестовые данные добавлены.");

        }
        
        private async Task SeedTestDataAircrafts()
        {
            var count = await _context.Aircrafts.CountAsync();
            _logger.LogInformation("📊 В базе {TodoCount} записей.", count);
            if (count > 0) return;
            _logger.LogInformation("➕ Добавляем тестовые данные...");
            var test = new List<Aircraft>
            {
                new() {Name="Airbus A320 VQ-BBA", Code="VQ-BBA", Status=AircraftsStatus.Active, AirportId=1, ModelId=1},
                new() {Name="Airbus A320 VQ-BBB", Code="VQ-BBB", Status=AircraftsStatus.Active, AirportId=1, ModelId=1},
                new() {Name="Airbus A320 VP-BLA", Code="VP-BLA", Status=AircraftsStatus.Active, AirportId=1, ModelId=1},
                new() {Name="Boeing 737-800 VP-BGJ", Code="VP-BGJ", Status=AircraftsStatus.Active, AirportId=1, ModelId=2},
                new() {Name="Boeing 737-800 VQ-BJI", Code="VQ-BJI", Status=AircraftsStatus.Active, AirportId=1, ModelId=2},
                new() {Name="Boeing 777-300ER RA-73299", Code="RA-73299", Status=AircraftsStatus.Active, AirportId=1, ModelId=3},
                new() {Name="Airbus A350-900 VQ-BFY", Code="VQ-BFY", Status=AircraftsStatus.Active, AirportId=1, ModelId=4},
                new() {Name="Airbus A350-900 VQ-BFZ", Code="VQ-BFZ", Status=AircraftsStatus.Active, AirportId=1, ModelId=4},
                new() {Name="Sukhoi Superjet 100 RA-89001", Code="RA-89001", Status=AircraftsStatus.Active, AirportId=1, ModelId=5},
                new() {Name="Sukhoi Superjet 100 RA-89002", Code="RA-89002", Status=AircraftsStatus.Active, AirportId=1, ModelId=5},
                new() {Name="Sukhoi Superjet 100 RA-89003", Code="RA-89003", Status=AircraftsStatus.Active, AirportId=1, ModelId=5},
            };
            
            await _context.Aircrafts.AddRangeAsync(test);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Тестовые данные добавлены.");

        }
        
        private async Task SeedTestDataFlights()
        {
            var count = await _context.Flights.CountAsync();
            _logger.LogInformation("📊 В базе {TodoCount} записей.", count);
            if (count > 0) return;
            _logger.LogInformation("➕ Добавляем тестовые данные...");
            
            var baseDate = DateTime.Now.Date;
            var test = new List<Flight>
            {
                new() {Number="SU1000", DepartureAirportId=1, ArrivalAirportId=5, Pattern=RecurrencePattern.Daily,
                        ScheduledDeparture = baseDate.AddHours(8).AddMinutes(0),   // 08:00
                        ScheduledArrival = baseDate.AddHours(9).AddMinutes(30),    // 09:30
                        },
                new() {Number="SU1001", DepartureAirportId=5, ArrivalAirportId=1, Pattern=RecurrencePattern.Daily,
                        ScheduledDeparture = baseDate.AddHours(10).AddMinutes(0),   // 10:00
                        ScheduledArrival = baseDate.AddHours(11).AddMinutes(30),    // 11:30
                        },
                new() {Number="SU2000", DepartureAirportId=2, ArrivalAirportId=6, Pattern=RecurrencePattern.Weekly,
                        ScheduledDeparture = baseDate.AddHours(12).AddMinutes(0),   // 12:00
                        ScheduledArrival = baseDate.AddHours(16).AddMinutes(30),    // 16:30
                       },
                
                new() {Number="SU2001", DepartureAirportId=6, ArrivalAirportId=2, Pattern=RecurrencePattern.Weekly,
                        ScheduledDeparture = baseDate.AddDays(1).AddHours(12).AddMinutes(0),   // 12:00
                        ScheduledArrival = baseDate.AddDays(1).AddHours(16).AddMinutes(30),    // 16:30
                       }
            };
            
            await _context.Flights.AddRangeAsync(test);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Тестовые данные добавлены.");

        }

        private async Task SeedTestDataFlightShedules()
        {
            var count = await _context.FlightShedules.CountAsync();
            _logger.LogInformation("📊 В базе {TodoCount} записей.", count);
            if (count > 0) return;
            _logger.LogInformation("➕ Добавляем тестовые данные...");
            
            var baseDate = DateTime.Now.Date;
            var test = new List<FlightShedule>
            {
                new() {FlightId = 1, AircraftId = 1, 
                        ActualDeparture = baseDate.AddHours(8).AddMinutes(0),   // 08:00
                        ActualArrival = baseDate.AddHours(9).AddMinutes(30),    // 09:30
                        Status=FlightStatus.Scheduled},
                new() {FlightId = 2, 
                        ActualDeparture = baseDate.AddHours(10).AddMinutes(0),   // 10:00
                        ActualArrival = baseDate.AddHours(11).AddMinutes(30),    // 11:30
                        Status=FlightStatus.Scheduled},
                new() {FlightId = 3, 
                        ActualDeparture = baseDate.AddHours(12).AddMinutes(0),   // 12:00
                        ActualArrival = baseDate.AddHours(16).AddMinutes(30),    // 16:30
                        Status=FlightStatus.Scheduled},
                new() {FlightId = 4, 
                        ActualDeparture = baseDate.AddDays(1).AddHours(12).AddMinutes(0),   // 12:00
                        ActualArrival = baseDate.AddDays(1).AddHours(16).AddMinutes(30),    // 16:30
                        Status=FlightStatus.Scheduled},
            };
            
            await _context.FlightShedules.AddRangeAsync(test);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Тестовые данные добавлены.");

        }
        private async Task SeedTestDataAsync()
        {
            await SeedTestDataUser();
            await SeedTestDataUserManager();
            await SeedTestDataCities();
            await SeedTestDataAirports();
            await SeedTestDataAircraftModels();
            await SeedTestDataAircrafts();
            await SeedTestDataFlights();
            await SeedTestDataFlightShedules();
        }
    }
}