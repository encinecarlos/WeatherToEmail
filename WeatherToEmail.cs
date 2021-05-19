using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using WeatherBasic.Weather;

namespace WeatherToEmail
{
    public static class WeatherToEmail
    {
        [FunctionName("WeatherToEmailFunction")]
        public static void Run([TimerTrigger("0 0 */4 * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation("Send weather information to e-mail");

                var client = new HttpClient();

                client.BaseAddress = new Uri("http://apiadvisor.climatempo.com.br");

                var local = Environment.GetEnvironmentVariable("locale");
                var token = Environment.GetEnvironmentVariable("token_api");
                var mailFrom = Environment.GetEnvironmentVariable("mail_from");
                var mailServer = Environment.GetEnvironmentVariable("mail_smtp");
                var mailPass = Environment.GetEnvironmentVariable("mail_pass");

                var response = client.GetAsync($"/api/v1/weather/locale/{local}/current?token={token}");
                response.Wait();

                var weatherResponse = new WeatherResult();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readResult = result.Content.ReadAsAsync<WeatherResult>();
                    readResult.Wait();

                    weatherResponse = readResult.Result;

                }

                using (var mailClient = new SmtpClient(mailServer))
                {
                    var basicCredentials = new NetworkCredential(mailFrom, mailPass);
                    using (var message = new MailMessage())
                    {
                        MailAddress from = new MailAddress(mailFrom, "Carlos Encine");
                        MailAddress to = new MailAddress("carlos_alexandre88@hotmail.com", "destination");

                        mailClient.Host = mailServer ?? "mail.carlosencine.com";
                        mailClient.UseDefaultCredentials = false;
                        mailClient.Credentials = basicCredentials;
                        message.From = from;
                        message.To.Add(to);
                        message.Subject = $"Clima agora: {DateTime.Now:dd/MM/yyyy hh:mm}";

                        string clima = $"Tempo na cidade de {weatherResponse.Name} - {weatherResponse.State}\n" +
                                       $"----------------------------------------------------------------------\n" +
                                       $"Temperatura: {weatherResponse.Data.Temperature}°C\n" +
                                       $"Humidade: {weatherResponse.Data.Humidity}%\n" +
                                       $"Condição: {weatherResponse.Data.Condition}\n" +
                                       $"Sensação Termica: {weatherResponse.Data.Sensation}°C";

                        message.Body = clima;
                        mailClient.Send(message);
                    }
                }

                log.LogInformation("Send to Email");
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
