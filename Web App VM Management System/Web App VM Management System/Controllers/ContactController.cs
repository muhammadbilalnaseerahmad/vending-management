
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using Web_App_VM_Management_System.dtos;

namespace Web_App_VM_Management_System.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Index(string message="",string color="")
        {
            ViewBag.message = message;
            ViewBag.color= color;
            return View();
        }
            [HttpPost]
        public IActionResult Index(EmailDTO emailDTO)
        {

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com");
                smtpClient.Authenticate("dev.hassan.naseer@gmail.com", "tgaw vzwk yhvw lqlx");

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("From", "dev.hassan.naseer@gmail.com"));


                message.To.Add(new MailboxAddress("To", $"fahadiqbal.fd14@gmail.com"));


                message.Subject = $"{emailDTO.Subject}";


                var textPart = new TextPart("plain")
                {
                    Text = emailDTO.Message
                };

                // Attach the text part to the message
                message.Body = textPart;

                // You can add more headers if needed
                //message.Headers.Add("X-Custom-Header", "SomeValue");

                smtpClient.Send(message);
                smtpClient.Disconnect(true);


            }
            ViewBag.message = "Message Sent!";
            ViewBag.color = "green";
            return View();
        }
    }
}
