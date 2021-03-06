using System.Text.Json.Serialization;

namespace Areks.Io.Exoskeleton.Models
{
    public record TelegramTextMessage
    {
        [JsonPropertyName("chat_id")]
        public long ChatId { get; init; }
            
        [JsonPropertyName("text")]
        public string Content { get; init; } = string.Empty;
        
        [JsonPropertyName("parse_mode")]
        public string ParseMode { get; init; } = "Markdown";
    }
}