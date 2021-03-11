using System.Threading.Tasks;
using Areks.Io.Exoskeleton.Models;

namespace Areks.Io.Exoskeleton.Services
{
    public interface ITelegramBot
    {
        Task SendMessage(TelegramTextMessage textMessage);
    }
}