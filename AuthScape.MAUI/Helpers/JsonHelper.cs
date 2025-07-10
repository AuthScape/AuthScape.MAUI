using System.Text.Json;

namespace AuthScape.MAUI.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public static T? Deserialize<T>(string json) =>
            JsonSerializer.Deserialize<T>(json, _options);

        public static string Serialize<T>(T obj) =>
            JsonSerializer.Serialize(obj, _options);
    }
}
