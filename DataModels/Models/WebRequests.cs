using System.Text.Json.Serialization;

namespace DataModels.Models
{
    public record WebhookCreatingPostJson
    {
        //[JsonPropertyName("key")] public string Key { get; init; }
        [JsonPropertyName("callbackURL")] public string CallbackUrl { get; init; }
        [JsonPropertyName("idModel")] public string IdModel { get; init; }
        [JsonPropertyName("description")] public string Description { get; init; }
    }

    public record GetTableParams
    {
        [JsonPropertyName("id")] public string TableId { get; init; }
    }

    public record UpdateTableParams
    {
        [JsonPropertyName("name")] public string NewName { get; init; }
    }
}