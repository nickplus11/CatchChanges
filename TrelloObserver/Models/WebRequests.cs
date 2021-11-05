using System.Text.Json.Serialization;

namespace TrelloObserver.Models
{
    public class WebRequests
    {
        public record WebhookCreatingPostJson
        {
            //[JsonPropertyName("key")] public string Key { get; init; }
            [JsonPropertyName("callbackURL")] public string CallbackUrl { get; init; }
            [JsonPropertyName("idModel")] public string IdModel { get; init; }
            [JsonPropertyName("description")] public string Description { get; init; }
        }
    }
}