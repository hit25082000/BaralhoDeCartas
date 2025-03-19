using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;

namespace BaralhoDeCartas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackjackController : ControllerBase
    {
        private readonly IBlackjackService _blackjackService;

        public BlackjackController(IBlackjackService blackjackService)
        {
            _blackjackService = blackjackService;
        }

        [HttpPost("iniciar")]
        public async Task<ActionResult<IBaralho>> IniciarJogo()
        {
            try
            {
                var baralho = await _blackjackService.IniciarJogo();
                return Ok(baralho);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar o jogo: {ex.Message}");
            }
        }

        [HttpPost("{deckId}/iniciar-rodada")]
        public async Task<ActionResult<List<IJogadorDeBlackjack>>> IniciarRodada(string deckId, [FromQuery] int numeroJogadores)
        {
            try
            {
                if (numeroJogadores <= 0)
                {
                    return BadRequest("O número de jogadores deve ser maior que zero.");
                }

                var jogadores = await _blackjackService.IniciarRodada(deckId, numeroJogadores);
                return Ok(jogadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar a rodada: {ex.Message}");
            }
        }

        [HttpPost("{deckId}/jogador/{jogadorId}/comprar")]
        public async Task<ActionResult<ICarta>> ComprarCarta(string deckId, int jogadorId, [FromBody] IJogadorDeBlackjack jogador)
        {
            try
            {
                if (jogador.JogadorId != jogadorId)
                {
                    return BadRequest("ID do jogador inválido.");
                }

                var novaCarta = await _blackjackService.ComprarCarta(deckId, jogador);

                return Ok(novaCarta);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao comprar carta: {ex.Message}");
            }
        }

        [HttpPost("{deckId}/jogador/{jogadorId}/parar")]
        public ActionResult<IJogadorDeBlackjack> PararJogador(int jogadorId, [FromBody] IJogadorDeBlackjack jogador)
        {
            try
            {
                if (jogador.JogadorId != jogadorId)
                {
                    return BadRequest("ID do jogador inválido.");
                }

                jogador.Parou = true;
                return Ok(jogador);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao parar jogador: {ex.Message}");
            }
        }

        [HttpPost("{deckId}/finalizar")]
        public async Task<ActionResult<List<IJogadorDeBlackjack>>> FinalizarRodada(string deckId, [FromBody] List<IJogadorDeBlackjack> jogadores)
        {
            try
            {
                var vencedores = _blackjackService.DeterminarVencedores(jogadores);
                await _blackjackService.FinalizarJogo(deckId);

                return Ok(new
                {
                    Vencedores = vencedores,
                    JogadoresFinais = jogadores
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao finalizar rodada: {ex.Message}");
            }
        }
    }
} 