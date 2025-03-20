using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Services.Interfaces;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Controllers   
{    
    public class JogoWebController : Controller
    {
        private readonly IMaiorCartaService _maiorCartaService;
        private readonly IBlackjackService _blackjackService;

        public JogoWebController(IMaiorCartaService jogoService, IBlackjackService blackjackService)
        {
            _maiorCartaService = jogoService;
            _blackjackService = blackjackService;
        }

        public async Task<IActionResult> Index(string jogo, int numeroJogadores)
        {
            if (string.IsNullOrEmpty(jogo))
            {
                return RedirectToAction("Index", "Jogos");
            }

            switch (jogo.ToLower())
            {
                case "maiorcarta":
                    var jogoMaiorCarta = await _maiorCartaService.CriarJogoMaiorCartaAsync(numeroJogadores);
                    return View("MaiorCarta", jogoMaiorCarta);
                
                case "blackjack":
                    var jogoBlackjack = await _blackjackService.CriarJogoBlackJackAsync(numeroJogadores);
                    return View("Blackjack", jogoBlackjack);
                
                default:
                    return RedirectToAction("Index", "Jogos");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DistribuirCartasAsync(string baralhoId, int numeroJogadores)
        {
            try
            {
                var jogadores = await _maiorCartaService.DistribuirCartasAsync(baralhoId, numeroJogadores);
                return Json(new { 
                    success = true, 
                    data = jogadores.Select(j => new {
                        id = j.JogadorId,
                        nome = j.Nome,
                        cartas = j.Cartas.Select(c => new {
                            valor = c.Valor,
                            valorSimbolico = c.ValorSimbolico,
                            naipe = c.Naipe
                        })
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> IniciarRodadaAsync(string baralhoId, int numeroJogadores)
        {
            var jogadores = await _blackjackService.IniciarRodadaAsync(baralhoId, numeroJogadores);
            return Json(jogadores);
        }

        [HttpGet]
        public async Task<IActionResult> ComprarCartaAsync(string baralhoId, IJogadorDeBlackjack jogadorId)
        {
            var jogador = await _blackjackService.ComprarCartaAsync(baralhoId, jogadorId);
            return Json(jogador);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarJogoAsync(string baralhoId)
        {
            await _maiorCartaService.FinalizarJogoAsync(baralhoId);
            return RedirectToAction("Index", "Jogos");
        }
    }
}
