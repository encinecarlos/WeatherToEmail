using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using WeatherToEmail.Weather;

namespace WeatherToEmail.Services
{
    public class SendMessage : ISendMessage
    {
        private string MailFrom { get; set; }
        private string MailTo { get; set; }
        public string ApiKey { get; set; }

        public SendMessage()
        {
            MailFrom = Environment.GetEnvironmentVariable("mail_from");
            MailTo = Environment.GetEnvironmentVariable("mail_to");
            ApiKey = Environment.GetEnvironmentVariable("SENDGRID_KEY");
        }


        public async void SendMessageToUser(WeatherResult weather)
        {
            try
            {
                var client = new SendGridClient(ApiKey);
                var from = new EmailAddress(MailFrom, "Weather Now");

                var weatherDate = Convert.ToDateTime(weather.Data.Date);

                var subject = $"Clima agora: {weatherDate:dd/MM/yyyy hh:mm}";
                var to = new EmailAddress(MailTo);
                var plainTextContent = $"Tempo na cidade de {weather.Name} - {weather.State}\n" +
                                       $"----------------------------------------------------------------------\n" +
                                       $"Temperatura: {weather.Data.Temperature}°C\n" +
                                       $"Humidade: {weather.Data.Humidity}%\n" +
                                       $"Condição: {weather.Data.Condition}\n" +
                                       $"Sensação Termica: {weather.Data.Sensation}°C";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, string.Empty);
                await client.SendEmailAsync(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



            //using (var mailClient = new SmtpClient(MailServer))
            //{
            //    var basicCredentials = new NetworkCredential(MailFrom, MailPass);
            //    using (var message = new MailMessage())
            //    {
            //        MailAddress from = new MailAddress(MailFrom, "Carlos Encine");
            //        MailAddress to = new MailAddress(MailTo, "destination");

            //        mailClient.Host = MailServer;
            //        mailClient.UseDefaultCredentials = false;
            //        mailClient.Credentials = basicCredentials;

            //        var weatherDate = Convert.ToDateTime(weather.Data.Date);

            //        message.From = from;
            //        message.To.Add(to);
            //        message.Subject = $"Clima agora: {weatherDate:dd/MM/yyyy hh:mm}";

            //        string clima = $"Tempo na cidade de {weather.Name} - {weather.State}\n" +
            //                       $"----------------------------------------------------------------------\n" +
            //                       $"Temperatura: {weather.Data.Temperature}°C\n" +
            //                       $"Humidade: {weather.Data.Humidity}%\n" +
            //                       $"Condição: {weather.Data.Condition}\n" +
            //                       $"Sensação Termica: {weather.Data.Sensation}°C";

            //        message.Body = clima;
            //        mailClient.Send(message);
            //    }
        }
    }
}