using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;

namespace BaralhoDeCartas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JogoApiController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogoApiController(IJogoService jogoService)
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

        [HttpPost("{baralhoId}/distribuir")]
        public async Task<ActionResult<List<IJogador>>> DistribuirCartas(string baralhoId, [FromQuery] int numeroJogadores)
        {
            try
            {
                if (numeroJogadores <= 0)
                {
                    return BadRequest("O número de jogadores deve ser maior que zero.");
                }

                var jogadores = await _jogoService.DistribuirCartas(baralhoId, numeroJogadores);
                return Ok(jogadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao distribuir as cartas: {ex.Message}");
            }
        }

        [HttpGet("/vencedor")]
        public async Task<ActionResult<IJogador>> ObterVencedor([FromBody] List<IJogador> jogadores)
        {
            try
            {
                if (jogadores == null || jogadores.Count == 0)
                {
                    return BadRequest(new { erro = "A lista de jogadores não pode estar vazia." });
                }

                var vencedor = await _jogoService.DeterminarVencedor(jogadores);
                return Ok(vencedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao determinar o vencedor: {ex.Message}");
            }
        }

        [HttpPost("{baralhoId}/finalizar")]
        public async Task<ActionResult<bool>> FinalizarJogo(string baralhoId)
        {
            try
            {
                var resultado = await _jogoService.FinalizarJogo(baralhoId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao finalizar o jogo: {ex.Message}");
            }
        }
    }
} 