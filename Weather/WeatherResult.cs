using System.Text.Json.Serialization;

namespace WeatherToEmail.Weather
{
    class WeatherResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
        public string Country { get; set; }
        public Data Data { get; set; }
    }
}
