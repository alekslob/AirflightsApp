using Airflights.Data;
using Airflights.Models;

namespace Airflights.Services
{
    public class PeriodicTaskService : BackgroundService
    {
        private readonly ILogger<PeriodicTaskService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer? _timer;
        public PeriodicTaskService(
            ILogger<PeriodicTaskService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 Периодический сервис запущен");
            
            // Запускаем таймер на каждую минуту
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            
            // Ждем отмены
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        private async void DoWork(object? state)
        {
            try
            {
                _logger.LogDebug("⏰ Запуск периодической задачи...");
                
                // Создаем новый scope для каждого выполнения
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var flightsShedulerService = scope.ServiceProvider.GetRequiredService<IFlightsShedulerService>();
                var flightService = scope.ServiceProvider.GetRequiredService<IFlightsService>();
                
                // обновление статусов рейсов
                await UpdateFlightStatusesAsync(flightsShedulerService, flightService);
                
                // генерация рейсов на следующий день
                await GenerateNextDayFlightsAsync(flightService, flightsShedulerService);
                
                _logger.LogDebug("✅ Периодическая задача завершена успешно");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Ошибка в периодической задаче");
            }
        }
        private async Task GenerateNextDayFlightsAsync(IFlightsService flightService, IFlightsShedulerService flightsShedulerService)
        {
            var tomorrow = DateTime.Today.AddDays(1);
            
            var items = await flightService.GetAllAsync(true);
            foreach (var item in items)
            {
                try
                {
                    var flight = await flightService.GetByIdAsync(item.Id);
                    if (flight == null) continue;
                    if (flight.Pattern == RecurrencePattern.Weekly && flight.ScheduledArrival.DayOfWeek != tomorrow.DayOfWeek) continue;
                    if (item.Pattern == RecurrencePattern.Specific && (tomorrow - flight.ScheduledArrival).Days!=0) continue;
                    var flightShedule = new FlightsShedulerCreateModel
                    {
                        FlightId = flight.Id,
                        ArrivalTime = flight.ScheduledArrival.AddDays((int)(tomorrow - flight.ScheduledArrival).Days),
                        DepartureTime = flight.ScheduledDeparture.AddDays((int)(tomorrow - flight.ScheduledDeparture).Days),
                    };
                    await flightsShedulerService.CreateAsync(flightShedule);
                }
                catch (System.Exception e)
                {
                    _logger.LogWarning($"Попытка создать рассписание на завтра: {e.Message}");
                }
            }
        }
        
        private async Task SetBoardingStatusesAsync(IFlightsShedulerService flightShedulerService)
        {
            //Посадка
            var currentDate =  DateTime.Now.Date;
            var items = await flightShedulerService.GetAllAsync(FlightStatus.Scheduled);
            foreach(var item in items)
            {
                try
                {
                    if (item.AircraftName != null && (item.DepartureTime-currentDate).TotalMinutes < 60){
                        _logger.LogInformation($"Рейс {item.Number}, самолет: {item.AircraftName}, статус: {item.Status} -> boarding");
                        var flightShedule = new FlightShedulerEditModel{Id=item.Id, StatusKey=(int)FlightStatus.Boarding};
                        await flightShedulerService.UpdateAsync(flightShedule);
                    }
                } catch (Exception e)
                {
                    _logger.LogWarning($"Попытка сменить статусы: {e.Message}");
                }
            }
        }
        private async Task SetDelayedStatusesAsync(IFlightsShedulerService flightShedulerService)
        {
            var currentDate =  DateTime.Now;
            _logger.LogInformation($"Проставление delayed {currentDate}");
            var items = await flightShedulerService.GetAllAsync(FlightStatus.Scheduled);
            foreach(var item in items)
            {
                try
                {
                    _logger.LogWarning($"Имя: {item.AircraftName}, время {item.DepartureTime}  : {(item.DepartureTime-currentDate)} ");
                    if (item.AircraftName == null && (item.DepartureTime-currentDate).TotalMinutes < 60){
                        var flightShedule = new FlightShedulerEditModel{Id=item.Id, StatusKey=(int)FlightStatus.Delayed};
                        await flightShedulerService.UpdateAsync(flightShedule);
                    }
                } catch (Exception e)
                {
                    _logger.LogWarning($"Попытка сменить статусы: {e.Message}");
                }
            }
        }
        private async Task SetCanseledStatusesAsync(IFlightsShedulerService flightsShedulerService, IFlightsService flightsService)
        {
            _logger.LogInformation($"Проставление canseled");
            var items = await flightsService.GetAllAsync(false);
            foreach(var item in items)
            {
                try
                {
                    _logger.LogWarning($"Имя: {item.Number}");
                    var schedulers = await flightsShedulerService.GetAllAsync(FlightId: item.Id);
                    foreach(var s in schedulers)
                    {
                        if (s.Status == FlightStatus.Scheduled){
                            var flightShedule = new FlightShedulerEditModel{Id=s.Id, StatusKey=(int)FlightStatus.Cancelled};
                            await flightsShedulerService.UpdateAsync(flightShedule);
                        }
                    }
                    
                } catch (Exception e)
                {
                    _logger.LogWarning($"Попытка сменить статусы: {e.Message}");
                }
            }
            
        }
        private async Task UpdateFlightStatusesAsync(IFlightsShedulerService flightShedulerService, IFlightsService flightsService)
        {
            //Посадка если самолет в наличии и подошло время
            await SetBoardingStatusesAsync(flightShedulerService);
            //Задержка если нет самолета и подошло время
            await SetDelayedStatusesAsync(flightShedulerService); 
            //Отмена если нет самолета и отменен маршрут
            await SetCanseledStatusesAsync(flightShedulerService, flightsService); 
        }
    }
}