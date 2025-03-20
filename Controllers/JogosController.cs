using Microsoft.AspNetCore.Mvc;

namespace BaralhoDeCartas.Controllers
{
    public class JogosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 