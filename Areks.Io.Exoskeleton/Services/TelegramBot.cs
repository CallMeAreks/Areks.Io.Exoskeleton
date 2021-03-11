using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Areks.Io.Exoskeleton.Models;
using Areks.Io.Exoskeleton.Settings;
using Microsoft.Extensions.Options;

namespace Areks.Io.Exoskeleton.Services
{
    public class TelegramBot : ITelegramBot
    {
        private TelegramBotSettings Settings { get; }
        private HttpClient HttpClient { get; }

        public TelegramBot(IOptions<TelegramBotSettings> settings, IHttpClientFactory httpClientFactory)
        {
            Settings = settings.Value;
            HttpClient = httpClientFactory.CreateClient();
        }
        
        public async Task SendMessage(TelegramTextMessage textMessage)
        {
            await HttpClient.PostAsJsonAsync(
                $"{Settings.BotApiUrl}/sendMessage", 
                textMessage
            );
        }
    }
}