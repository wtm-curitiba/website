using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;

namespace WomenTechmakers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home page hit!");
            return View();
        }

        [HttpGet]
        public JsonResult SendEmail(string email, string name, string message)
        {
            if (email == null || email.Equals(""))
            {
                return Json(new { Processado = false, Mensagem = "Ocorreu um erro! Preencha corretamente o campo Email." });
            }
            if (name == null || name.Equals(""))
            {
                return Json(new { Processado = false, Mensagem = "Ocorreu um erro! Preencha corretamente o campo Nome." });
            }
            if (message == null || message.Equals(""))
            {
                return Json(new { Processado = false, Mensagem = "Ocorreu um erro! Preencha corretamente o campo Mensagem." });
            }

            try
            {
                var messageMime = new MimeMessage();
                messageMime.From.Add(new MailboxAddress("WTM Site", "wtmcuritiba@gmail.com"));
                messageMime.To.Add(new MailboxAddress("WTM Curitiba", "wtmcuritiba@gmail.com"));
                messageMime.Subject = "Contato - site";

                messageMime.Body = new TextPart("plain")
                {
                    Text = @"Novo contato - Nome: " + name + " Email: " + email + " Mensagem:" + message
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate("wtmcuritiba@gmail.com", "WTMCuritiba2015");

                    client.Send(messageMime);
                    client.Disconnect(true);
                }

                return Json(new { Processado = true, Mensagem = "Obrigado pelo contato! Assim que possível responderemos sua mensagem. :D" + message });
            }
            catch (Exception e)
            {
                return Json(new { Processado = false, Mensagem = "Ocorreu um erro com nosso servidor! Tente novamente mais tarde." + e.Message });
            }

        }
    }
}
