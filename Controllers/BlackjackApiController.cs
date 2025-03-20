using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;

namespace BaralhoDeCartas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackjackApiController : ControllerBase
    {
        private readonly IBlackjackService _blackjackService;

        public BlackjackApiController(IBlackjackService blackjackService)
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

        [HttpPost("{baralhoId}/iniciar-rodada/{numeroJogadores}")]
        public async Task<ActionResult<List<IJogadorDeBlackjack>>> IniciarRodada(string baralhoId, int numeroJogadores)
        {
            try
            {
                if (numeroJogadores <= 0)
                {
                    return BadRequest("O número de jogadores deve ser maior que zero.");
                }

                var jogadores = await _blackjackService.IniciarRodada(baralhoId, numeroJogadores);
                return Ok(jogadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar a rodada: {ex.Message}");
            }
        }

        [HttpPost("{baralhoId}/jogador/comprar")]
        public async Task<ActionResult<ICarta>> ComprarCarta(string baralhoId, [FromBody] IJogadorDeBlackjack jogador)
        {
            try
            {
                var novaCarta = await _blackjackService.ComprarCarta(baralhoId, jogador);

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

        [HttpPost("{baralhoId}/jogador/{jogadorId}/parar")]
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

        [HttpPost("{baralhoId}/finalizar")]
        public async Task<ActionResult<List<IJogadorDeBlackjack>>> FinalizarRodada(string baralhoId, [FromBody] List<IJogadorDeBlackjack> jogadores)
        {
            try
            {
                var vencedores = _blackjackService.DeterminarVencedores(jogadores);
                await _blackjackService.FinalizarJogo(baralhoId);

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