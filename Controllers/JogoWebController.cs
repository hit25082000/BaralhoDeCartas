using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Services.Interfaces;

namespace BaralhoDeCartas.Controllers   
{    
    public class JogoWebController : Controller
    {
        private readonly IJogoService _jogoService;

        public JogoWebController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        public async Task<IActionResult> Index()
        {
            var jogo = await _jogoService.IniciarNovoJogo();
            return View(jogo); 
        }
    }
}
