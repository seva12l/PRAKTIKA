using System;

/*
 * Задание 4.
 * Реализуем паттерн Издатель-Подписчик с использованием EventHandler.
 * WeatherStation - издатель, генерирует событие WeatherChanged.
 * WeatherMonitor - промежуточный класс, подписывает подписчиков на событие.
 * DisplayPanel обновляет данные о погоде.
 * WarningSystem предупреждает о шторме если ветер сильный.
*/

class WeatherEventArgs : EventArgs
{
    public string Weather;
    public int WindSpeed;

    public WeatherEventArgs(string weather, int windSpeed)
    {
        Weather = weather;
        WindSpeed = windSpeed;
    }
}

class WeatherStation
{
    public event EventHandler<WeatherEventArgs> WeatherChanged;

    public void ChangeWeather(string weather, int windSpeed)
    {
        Console.WriteLine("Погода изменилась: " + weather + ", ветер: " + windSpeed + " км/ч");
        if (WeatherChanged != null)
            WeatherChanged(this, new WeatherEventArgs(weather, windSpeed));
    }
}

class DisplayPanel
{
    public void UpdateDisplay(object sender, WeatherEventArgs e)
    {
        Console.WriteLine("DisplayPanel: обновление данных - " + e.Weather + ", ветер: " + e.WindSpeed + " км/ч");
    }
}

class WarningSystem
{
    public void CheckStorm(object sender, WeatherEventArgs e)
    {
        if (e.WindSpeed > 60)
            Console.WriteLine("WarningSystem: ВНИМАНИЕ! Штормовое предупреждение! Ветер " + e.WindSpeed + " км/ч");
        else
            Console.WriteLine("WarningSystem: погода в норме, шторма нет");
    }
}

class WeatherMonitor
{
    public WeatherMonitor(WeatherStation station, DisplayPanel display, WarningSystem warning)
    {
        station.WeatherChanged += display.UpdateDisplay;
        station.WeatherChanged += warning.CheckStorm;
    }
}

class Program
{
    static void Main()
    {
        WeatherStation station = new WeatherStation();
        DisplayPanel display = new DisplayPanel();
        WarningSystem warning = new WarningSystem();

        WeatherMonitor monitor = new WeatherMonitor(station, display, warning);

        station.ChangeWeather("Облачно", 30);
        Console.WriteLine();
        station.ChangeWeather("Гроза", 80);

        Console.ReadKey();
    }
}