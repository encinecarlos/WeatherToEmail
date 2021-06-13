using WeatherToEmail.Weather;

namespace WeatherToEmail.Services
{
    public interface ISendMessage
    {
        public void SendMessageToUser(WeatherResult weather);
    }
}