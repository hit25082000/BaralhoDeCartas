using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;
using BaralhoDeCartas.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using BaralhoDeCartas.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BaralhoDeCartas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaiorCartaApiController : ControllerBase
    {
        private readonly IMaiorCartaService _jogoService;

        public MaiorCartaApiController(IMaiorCartaService jogoService)
        {
            _jogoService = jogoService;
        }

        [HttpGet("iniciar")]
        public async Task<ActionResult<IBaralho>> IniciarJogoAsync()
        {
            try
            {
                var baralho = await _jogoService.CriarNovoBaralhoAsync();
                return Ok(baralho);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar o jogo: {ex.Message}");
            }
        }

        [HttpPost("{baralhoId}/distribuir/{numeroJogadores}")]
        public async Task<ActionResult<List<JogadorDTO>>> DistribuirCartasAsync(string baralhoId, int numeroJogadores)
        {
            try
            {
                if (string.IsNullOrEmpty(baralhoId))
                {
                    return BadRequest("ID do baralho não pode ser nulo ou vazio");
                }

                if (numeroJogadores <= 0)
                {
                    return BadRequest("O número de jogadores deve ser maior que zero.");
                }

                var jogadores = await _jogoService.DistribuirCartasAsync(baralhoId, numeroJogadores);
                var jogadoresDTO = JogadorDTO.FromJogadores(jogadores);
                return Ok(jogadoresDTO);
            }
            catch (ExternalServiceUnavailableException ex)
            {
                return StatusCode(503, "Serviço externo indisponível: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao distribuir as cartas: {ex.Message}");
            }
        }

        [HttpPost("/vencedor")]
        public async Task<ActionResult<JogadorDTO>> ObterVencedorAsync([FromBody] List<JogadorDTO> jogadoresDTO)
        {
            try
            {
                if (jogadoresDTO == null || jogadoresDTO.Count == 0)
                {
                    return BadRequest(new { erro = "A lista de jogadores não pode estar vazia." });
                }

                var jogadores = JogadorDTO.ToJogadores(jogadoresDTO);
                var vencedor = await _jogoService.DeterminarVencedorAsync(jogadores);
                return Ok(new JogadorDTO(vencedor));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao determinar o vencedor: {ex.Message}");
            }
        }

        [HttpPost("{baralhoId}/finalizar")]
        public async Task<ActionResult<bool>> FinalizarJogosync(string baralhoId)
        {
            try
            {
                if (string.IsNullOrEmpty(baralhoId))
                {
                    throw new ArgumentException("ID do baralho não pode ser nulo ou vazio", nameof(baralhoId));
                }

                var resultado = await _jogoService.FinalizarJogoAsync(baralhoId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao finalizar o jogo: {ex.Message}");
            }
        }
    }
} 