using CLabManager_API.Services;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;

namespace CLabManager_API.Controllers
{
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(MailData mailData)
        {
            bool result = await _mailService.SendAsync(mailData, new CancellationToken());

            if (result)
                return StatusCode(StatusCodes.Status200OK, "Email has been sent");
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred, email not sent");
        }
        
        [HttpPost("emailWithAttachments")]
        public async Task<IActionResult> SendEmailWithAttachments([FromForm]MailDataWithAttachments mailData)
        {
            bool result = await _mailService.SendWithAttachmentAsync(mailData, new CancellationToken());

            if (result)
                return StatusCode(StatusCodes.Status200OK, "Email with attachment has been sent");
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred, email with attachment not sent");
        }

        
        [HttpPost("emailWithTemplate")]
        public async Task<IActionResult> SendEmailWithTemplate(WelcomeMail welcomeMail)
        {
            MailData mailData = new(
                new List<string> { welcomeMail.Email }, 
                "Welcome to the MailKit Demo", 
                _mailService.GetEmailTemplate("welcome", welcomeMail));

            bool sendResult = await _mailService.SendAsync(mailData, new CancellationToken());
            if (sendResult)
                return StatusCode(StatusCodes.Status200OK, "Email with template has been sent");
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred, email with template not sent");
        }
    }
}
