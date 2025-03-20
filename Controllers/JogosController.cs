using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Services.Interfaces;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Controllers
{
    public class JogosController : Controller
    {
        private readonly IBlackjackService _blackjackService;

        public JogosController(IBlackjackService blackjackService)
        {
            _blackjackService = blackjackService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Blackjack()
        {
            var jogo = await _blackjackService.CriarJogoBlackJackAsync(2);
            return View(jogo);
        }
    }
} 