using EmailSend_SendGrid_WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Threading.Tasks;


namespace EmailSend_SendGrid_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailController : ControllerBase
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;
        public SendMailController(
            ISendGridClient sendGridClient,
            IConfiguration configuration)
        {
            _sendGridClient = sendGridClient;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("Text-Mail")]
        public async Task<IActionResult> SendPlainTextEmail(string toEmail)
        {
            string fromEmail = _configuration.GetSection("EmailSettings")
            .GetValue<string>("FromEmail");

            string fromName = _configuration.GetSection("EmailSettings")
            .GetValue<string>("FromName");

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = "Send  New Mail",
                PlainTextContent = "Hii iam Soubhagya..."
            };
            msg.AddTo(toEmail);
            
			var response = await _sendGridClient.SendEmailAsync(msg);
            string message = response.IsSuccessStatusCode ? "Email Success Send.." :
            "Sending Unsuccessful..";
            return Ok(message);
        }

		[HttpPost]
		[Route("send-html-mail")]
		public async Task<IActionResult> SendHtmlEmail(DynamicEmailSend heroEmail)
		{
			string fromEmail = _configuration.GetSection("EmailSettings")
			.GetValue<string>("FromEmail");

			string fromName = _configuration.GetSection("EmailSettings")
			.GetValue<string>("FromName");

			var msg = new SendGridMessage()
			{
				From = new EmailAddress(fromEmail, fromName),
				Subject = "HTML Email",
                HtmlContent =string.Format(heroEmail.FirstName )
                   



        };
            string FilePathname = Directory.GetCurrentDirectory() + "\\Template\\EmailTemplate\\MailTemplate.html";
            string EmailTemplateText = System.IO.File.ReadAllText(FilePathname);
            EmailTemplateText = Convert.ToString((EmailTemplateText, DateTime.Now.Date.ToLongDateString()));
            msg.HtmlContent = EmailTemplateText.Replace("name",
            heroEmail.FirstName +" "+heroEmail.LastName);
            
            msg.AddTo(heroEmail.ToEmail);
			var response = await _sendGridClient.SendEmailAsync(msg);
			string message = response.IsSuccessStatusCode ? "Email Send Success" :
			"Email Sending Failed";
			return Ok(message);
		}
	}
}
