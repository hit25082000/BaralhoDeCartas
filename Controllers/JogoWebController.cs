using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Services.Interfaces;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Models.ViewModel;

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
            // Garantir que haja pelo menos 2 jogadores (computador + 1 humano)
            if (numeroJogadores < 2)
            {
                numeroJogadores = 2;
            }
            
            // Limitar o número máximo de jogadores para 6 para garantir boa experiência de usuário
            if (numeroJogadores > 6)
            {
                numeroJogadores = 6;
            }

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
        public async Task<IActionResult> DistribuirCartas(string baralhoId, int numeroJogadores)
        {
            IBaralho baralho = null;
            List<IJogador> jogadores = null;
            int tentativas = 0;
            const int maxTentativas = 3;

            while (tentativas < maxTentativas)
            {
                try
                {
                    // Obtenha informações sobre o baralho
                    baralho = await _maiorCartaService.VerificarBaralhoAsync(baralhoId);
                    
                    // Se for uma nova partida ou o baralho estiver com poucas cartas, embaralhe-o
                    if (baralho.QuantidadeDeCartasRestantes < numeroJogadores * 5) // 5 cartas por jogador
                    {
                        // Devolver todas as cartas ao baralho
                        baralho = await _maiorCartaService.FinalizarJogoAsync(baralhoId);
                        
                        // Embaralhar o baralho todo
                        baralho = await _maiorCartaService.EmbaralharBaralhoAsync(baralhoId, false);
                    }
                    
                    // Agora tente distribuir as cartas
                    jogadores = await _maiorCartaService.DistribuirCartasAsync(baralhoId, numeroJogadores);
                    
                    // Se chegou aqui, tudo funcionou
                    return Json(new { 
                        success = true, 
                        data = jogadores.Select(j => new {
                            id = j.JogadorId,
                            nome = j.Nome,
                            cartas = j.Cartas.Select(c => new {
                                valor = c.Valor,
                                valorSimbolico = c.ValorSimbolico,
                                naipe = c.Naipe,
                                imagem = c.ImagemUrl
                            })
                        })
                    });
                }
                catch (Exception ex)
                {
                    tentativas++;
                    
                    // Se for erro de "Falha ao comprar cartas" ou estiver na última tentativa
                    if (ex.Message.Contains("Falha ao comprar cartas") || tentativas >= maxTentativas)
                    {
                        try
                        {
                            // Criar um novo baralho ao invés de tentar consertar o atual
                            baralho = await _maiorCartaService.CriarNovoBaralhoAsync();
                            baralhoId = baralho.BaralhoId; // Atualizar o ID do baralho
                            
                            // Tentar novamente com o novo baralho
                            jogadores = await _maiorCartaService.DistribuirCartasAsync(baralhoId, numeroJogadores);
                            
                            return Json(new { 
                                success = true,
                                novoBaralho = true,
                                baralhoId = baralhoId,
                                data = jogadores.Select(j => new {
                                    id = j.JogadorId,
                                    nome = j.Nome,
                                    cartas = j.Cartas.Select(c => new {
                                        valor = c.Valor,
                                        valorSimbolico = c.ValorSimbolico,
                                        naipe = c.Naipe,
                                        imagem = c.ImagemUrl
                                    })
                                })
                            });
                        }
                        catch (Exception innerEx)
                        {
                            // Se falhar mesmo com o novo baralho, retorne o erro
                            return Json(new { success = false, error = $"Erro após criar novo baralho: {innerEx.Message}" });
                        }
                    }
                    
                    // Se não for o último erro, continue tentando
                    continue;
                }
            }
            
            // Se chegou aqui, esgotou as tentativas
            return Json(new { success = false, error = "Não foi possível distribuir as cartas após várias tentativas." });
        }

        [HttpPost]
        public IActionResult RenderizarCarta([FromBody] CartaViewModel carta)
        {
            return PartialView("_CartaPartial", carta);
        }

        [HttpGet]
        public async Task<IActionResult> IniciarRodada(string baralhoId, int numeroJogadores)
        {
            var jogadores = await _blackjackService.IniciarRodadaAsync(baralhoId, numeroJogadores);
            return Json(jogadores);
        }

        [HttpGet]
        public async Task<IActionResult> ComprarCarta(string baralhoId, IJogadorDeBlackjack jogadorId)
        {
            var jogador = await _blackjackService.ComprarCartaAsync(baralhoId, jogadorId);
            return Json(jogador);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarJogo(string baralhoId)
        {
            await _maiorCartaService.FinalizarJogoAsync(baralhoId);
            return RedirectToAction("Index", "Jogos");
        }
    }
}
