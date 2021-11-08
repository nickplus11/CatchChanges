using System.Text.Json.Serialization;

namespace CatchChangesREST.Clients.Telegram
{
    public record Update
    {
        [JsonPropertyName("update_id")] public int UpdateId { get; set; }

        [JsonPropertyName("message")] public Message Message { get; set; }
    }

    public record Message
    {
        [JsonPropertyName("message_id")] public int MessageId { get; set; }

        [JsonPropertyName("from")] public User From { get; set; }

        [JsonPropertyName("chat")] public Chat Chat { get; set; }

        [JsonPropertyName("text")] public string Text { get; set; }
    }

    public record Chat
    {
        [JsonPropertyName("id")] public int Id { get; set; }

        [JsonPropertyName("first name")] public string FirstName { get; set; }

        [JsonPropertyName("last_name")] public string LastName { get; set; }

        [JsonPropertyName("username")] public string Username { get; set; }

        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("data")] public long Data { get; set; }
    }

    public record User
    {
        [JsonPropertyName("id")] public int Id { get; set; }

        [JsonPropertyName("is_bot")] public bool IsBot { get; set; }

        [JsonPropertyName("first_name")] public string FirstName { get; set; }

        [JsonPropertyName("last_name")] public string LastName { get; set; }

        [JsonPropertyName("username")] public string Username { get; set; }

        [JsonPropertyName("language_code")] public string LanguageCode { get; set; }
    }

    public record SendMessageModel
    {
        [JsonPropertyName("chat_id")] public int ChatId { get; set; }
        [JsonPropertyName("text")] public string Text { get; set; }
    }
}