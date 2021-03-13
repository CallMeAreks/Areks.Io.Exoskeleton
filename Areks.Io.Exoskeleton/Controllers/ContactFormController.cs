using System.Threading.Tasks;
using Areks.Io.Exoskeleton.Models;
using Areks.Io.Exoskeleton.Services;
using Microsoft.AspNetCore.Mvc;

namespace Areks.Io.Exoskeleton.Controllers
{
    [ApiController]
    [Route("api/contact")]
    public class ContactFormController : ControllerBase
    {
        private ITelegramBot Bot { get; }
        
        public ContactFormController(ITelegramBot bot)
        {
            Bot = bot;
        }

        [HttpPost]
        [Route("{formId}")]
        public async Task<IActionResult> SendMessage(long formId, [FromQuery]string? returnUrl)
        {
            var formParser = new ContactFormParser(Request.Form);

            if (!formParser.Succeeded)
            {
                return BadRequest();
            }
            
            await Bot.SendMessage(new TelegramTextMessage { ChatId = formId, Content = formParser.ToString()});

            if (returnUrl is null)
            {
                return Ok();
            }
            
            return Redirect(returnUrl);
        }
    }
}