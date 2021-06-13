using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using WeatherToEmail.Services;
using WeatherToEmail.Weather;

namespace WeatherToEmail
{
    public static class WeatherToEmail
    {
        [FunctionName("WeatherToEmailFunction")]
        public static void Run([TimerTrigger("0 0 */4 * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation("Get current Weather");
                var weather = new WeatherInfo();

                var currentWeather = weather.GetCurrentWeather();
                
                log.LogInformation("Send to Email");
                var sendWeather = new SendMessage();

                sendWeather.SendMessageToUser(currentWeather);
            }
            catch (Exception e)
            {
                log.LogError(e.Message, e.StackTrace);
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
