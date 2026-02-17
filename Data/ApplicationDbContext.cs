using Microsoft.EntityFrameworkCore;
using Airflights.Models;

namespace Airflights.Data
{
    
    public class ApplicationDbContext : DbContext
    {
        private readonly ILogger<ApplicationDbContext> _logger;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ILogger<ApplicationDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }
        public DbSet<City> Cities => Set<City>();
        public DbSet<Airport> Airports => Set<Airport>();
        public DbSet<AircraftModel> AircraftModels => Set<AircraftModel>();
        public DbSet<Aircraft> Aircrafts => Set<Aircraft>();
        public DbSet<Flight> Flights => Set<Flight>();
        public DbSet<FlightShedule> FlightShedules => Set<FlightShedule>();
        public DbSet<User> Users => Set<User>();
        // public DbSet<Todo> Todos => Set<Todo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Уникальность IATA кода аэропорта
            modelBuilder.Entity<Airport>()
                .HasIndex(a => a.IadaCode)
                .IsUnique();

            // Уникальность IATA кода города
            modelBuilder.Entity<City>()
                .HasIndex(c => c.IadaCode)
                .IsUnique();
            modelBuilder.Entity<AircraftModel>()
                .HasIndex(a => a.Name)
                .IsUnique();
            modelBuilder.Entity<Aircraft>()
                .HasIndex(a => a.Code)
                .IsUnique();
            modelBuilder.Entity<Flight>()
                .HasIndex(f => f.Number)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

        }
        
    }
    
}