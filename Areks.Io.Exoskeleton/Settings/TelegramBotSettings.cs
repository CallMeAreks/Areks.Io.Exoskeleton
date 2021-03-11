namespace Areks.Io.Exoskeleton.Settings
{
    public record TelegramBotSettings : BaseSettings
    {
        public string Name { get; init; } = string.Empty;
        public string ApiKey { get; init; } = string.Empty;
        public string BotApiUrl => $"https://api.telegram.org/bot{ApiKey}";
    }
}