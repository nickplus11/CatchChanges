using System.Text.Json.Serialization;

namespace TrelloObserver.Models
{
    public record Table
    {
        [JsonPropertyName("name")] public string Name { get; init; }
        [JsonPropertyName("id")] public string Id { get; init; }
    }

    public record List
    {
        [JsonPropertyName("name")] public string Name { get; init; }
        [JsonPropertyName("id")] public string Id { get; init; }
        [JsonPropertyName("idTable")] public string TableId { get; init; }
    }

    public record Card
    {
        [JsonPropertyName("name")] public string Name { get; init; }
        [JsonPropertyName("id")] public string Id { get; init; }
        [JsonPropertyName("idList")] public string ListId { get; init; }
    }
}