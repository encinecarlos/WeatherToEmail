using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherToEmail.Weather;

namespace WeatherToEmail.Services
{
    public class WeatherInfo : IWeatherInfo
    {
        private string Local { get; set; }
        private string Token { get; set; }
        private string UrlBase { get; set; }

        public WeatherInfo()
        {
            Local = Environment.GetEnvironmentVariable("locale");
            Token = Environment.GetEnvironmentVariable("token_api");
            UrlBase = Environment.GetEnvironmentVariable("weather_url");
        }

        public WeatherResult GetCurrentWeather()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(UrlBase)
            };

            var response = client.GetAsync($"/api/v1/weather/locale/{Local}/current?token={Token}");
            response.Wait();

            var weatherResponse = new WeatherResult();
            
            if (response.Result.IsSuccessStatusCode)
            {
                var readResult = response.Result.Content.ReadAsAsync<WeatherResult>();
                readResult.Wait();

                weatherResponse = readResult.Result;
            }

            return weatherResponse;
        }
    }
}