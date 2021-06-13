using System;
using System.Net;
using System.Net.Mail;
using WeatherToEmail.Weather;

namespace WeatherToEmail.Services
{
    public class SendMessage : ISendMessage
    {
        private string MailFrom { get; set; }
        private string MailTo { get; set; }
        private string MailServer { get; set; }
        private string MailPass { get; set; }

        public SendMessage()
        {
            MailFrom = Environment.GetEnvironmentVariable("mail_from");
            MailTo = Environment.GetEnvironmentVariable("mail_to");
            MailServer = Environment.GetEnvironmentVariable("mail_smtp");
            MailPass = Environment.GetEnvironmentVariable("mail_pass");
        }


        public void SendMessageToUser(WeatherResult weather)
        {
            using (var mailClient = new SmtpClient(MailServer))
            {
                var basicCredentials = new NetworkCredential(MailFrom, MailPass);
                using (var message = new MailMessage())
                {
                    MailAddress from = new MailAddress(MailFrom, "Carlos Encine");
                    MailAddress to = new MailAddress(MailTo, "destination");

                    mailClient.Host = MailServer;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Credentials = basicCredentials;

                    var weatherDate = Convert.ToDateTime(weather.Data.Date);

                    message.From = from;
                    message.To.Add(to);
                    message.Subject = $"Clima agora: {weatherDate:dd/MM/yyyy hh:mm}";

                    string clima = $"Tempo na cidade de {weather.Name} - {weather.State}\n" +
                                   $"----------------------------------------------------------------------\n" +
                                   $"Temperatura: {weather.Data.Temperature}°C\n" +
                                   $"Humidade: {weather.Data.Humidity}%\n" +
                                   $"Condição: {weather.Data.Condition}\n" +
                                   $"Sensação Termica: {weather.Data.Sensation}°C";

                    message.Body = clima;
                    mailClient.Send(message);
                }
            }
        }
    }
}