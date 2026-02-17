namespace Airflights
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Создаем и запускаем приложение
            var app = new AirflightsApp(args);
            app.Run();
        }
    }
}