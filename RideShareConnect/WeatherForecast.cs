namespace RideShareConnect
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC * 9 / 5); // Corrected formula
        public string? Summary { get; set; }
    }
}