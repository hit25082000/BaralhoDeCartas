using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Services.Interfaces;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Controllers   
{    
    public class JogoWebController : Controller
    {
        private readonly IMaiorCartaService _jogoService;
        private readonly IBlackjackService _blackjackService;

        public JogoWebController(IMaiorCartaService jogoService, IBlackjackService blackjackService)
        {
            _jogoService = jogoService;
            _blackjackService = blackjackService;
        }

        public async Task<IActionResult> Index(string jogo)
        {
            if (string.IsNullOrEmpty(jogo))
            {
                return RedirectToAction("Index", "Jogos");
            }

            switch (jogo.ToLower())
            {
                case "maiorcarta":
                    var jogoMaiorCarta = await _jogoService.IniciarNovoJogo();
                    return View("MaiorCarta", jogoMaiorCarta);
                
                case "blackjack":
                    var jogoBlackjack = await _blackjackService.IniciarJogo();
                    return View("Blackjack", jogoBlackjack);
                
                default:
                    return RedirectToAction("Index", "Jogos");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DistribuirCartas(string baralhoId, int numeroJogadores)
        {
            var jogadores = await _jogoService.DistribuirCartas(baralhoId, numeroJogadores);
            return Json(jogadores);
        }

        [HttpGet]
        public async Task<IActionResult> IniciarRodada(string baralhoId, int numeroJogadores)
        {
            var jogadores = await _blackjackService.IniciarRodada(baralhoId, numeroJogadores);
            return Json(jogadores);
        }

        [HttpGet]
        public async Task<IActionResult> ComprarCarta(string baralhoId, IJogadorDeBlackjack jogadorId)
        {
            var jogador = await _blackjackService.ComprarCarta(baralhoId, jogadorId);
            return Json(jogador);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarJogo(string baralhoId)
        {
            await _jogoService.FinalizarJogo(baralhoId);
            return RedirectToAction("Index", "Jogos");
        }
    }
}
