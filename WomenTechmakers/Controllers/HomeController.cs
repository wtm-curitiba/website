using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using WomenTechmakers.VIewModels;

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

        [HttpPost]
        public JsonResult SendEmail(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(new { Processado = true, Mensagem = "Obrigado pelo contato! Logo responderemos sua mensagem." });
            }
            else
            {
                return Json(new { Processado = false, Mensagem = "Preencha todos os campos corretamente." });
            }

        }
    }
}
