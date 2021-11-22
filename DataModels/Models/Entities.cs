using System.Text.Json.Serialization;

namespace DataModels.Models
{
    public record Table
    {
        [JsonPropertyName("name")] public string Name { get; init; }
        [JsonPropertyName("id")] public string Id { get; init; }
    }

    /*public record List
    {
        [JsonPropertyName("name")] public string Name { get; init; }
        [JsonPropertyName("id")] public string Id { get; init; }
        [JsonPropertyName("idTable")] public string TableId { get; init; }
    }*/

    /*public record Card
    {
        [JsonPropertyName("name")] public string Name { get; init; }
        [JsonPropertyName("id")] public string Id { get; init; }
        [JsonPropertyName("idList")] public string ListId { get; init; }
    }*/

    public record Credentials(string TrelloKey, string TrelloToken, string TelegramToken);

    public record ClientUpdate()
    {
        [JsonPropertyName("update_id")] public string UpdateId { get; init; }
        [JsonPropertyName("message")] public string Message { get; init; }
    }
}