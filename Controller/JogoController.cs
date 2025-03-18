using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Services;
using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogoController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        [HttpPost("iniciar")]
        public async Task<ActionResult<IBaralho>> IniciarJogo()
        {
            try
            {
                var baralho = await _jogoService.IniciarNovoJogo();
                return Ok(baralho);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar o jogo: {ex.Message}");
            }
        }

        [HttpPost("{deckId}/distribuir")]
        public async Task<ActionResult<List<Jogador>>> DistribuirCartas(string deckId, [FromQuery] int numeroJogadores)
        {
            try
            {
                if (numeroJogadores <= 0)
                {
                    return BadRequest("O número de jogadores deve ser maior que zero.");
                }

                var jogadores = await _jogoService.DistribuirCartas(deckId, numeroJogadores);
                return Ok(jogadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao distribuir as cartas: {ex.Message}");
            }
        }

        [HttpGet("{deckId}/vencedor")]
        public async Task<ActionResult<Jogador>> ObterVencedor([FromBody] List<Jogador> jogadores)
        {
            try
            {
                if (jogadores == null || !jogadores.Any())
                {
                    return BadRequest("A lista de jogadores não pode estar vazia.");
                }

                var vencedor = await _jogoService.DeterminarVencedor(jogadores);
                return Ok(vencedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao determinar o vencedor: {ex.Message}");
            }
        }

        [HttpPost("{deckId}/finalizar")]
        public async Task<ActionResult<bool>> FinalizarJogo(string deckId)
        {
            try
            {
                var resultado = await _jogoService.FinalizarJogo(deckId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao finalizar o jogo: {ex.Message}");
            }
        }
    }
} 