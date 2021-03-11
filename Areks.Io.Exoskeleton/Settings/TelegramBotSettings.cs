namespace Areks.Io.Exoskeleton.Settings
{
    public record TelegramBotSettings : BaseSettings
    {
        public string Name { get; init; }
        public string ApiKey { get; init; }
        public string BotApiUrl => $"https://api.telegram.org/bot{ApiKey}";
    }
}