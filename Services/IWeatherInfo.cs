using System.Net.Http;
using System.Threading.Tasks;
using WeatherToEmail.Weather;

namespace WeatherToEmail.Services
{
    public interface IWeatherInfo
    {
        WeatherResult GetCurrentWeather();
    }
}