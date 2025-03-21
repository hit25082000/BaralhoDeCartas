using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;
using BaralhoDeCartas.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using BaralhoDeCartas.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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
            catch (ExternalServiceUnavailableException ex)
            {
                return StatusCode(503, new { erro = "Serviço temporariamente indisponível", detalhes = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Ocorreu um erro interno no servidor" });
            }
        }

        [HttpPost("{baralhoId}/distribuir/{numeroJogadores}")]
        public async Task<ActionResult<List<JogadorDTO>>> DistribuirCartasAsync(string baralhoId, int numeroJogadores)
        {
            if (string.IsNullOrEmpty(baralhoId))
            {
                return BadRequest(new { erro = "ID do baralho não pode ser nulo ou vazio" });
            }

            if (numeroJogadores <= 0)
            {
                return BadRequest(new { erro = "O número de jogadores deve ser maior que zero" });
            }

            try
            {
                var jogadores = await _jogoService.DistribuirCartasAsync(baralhoId, numeroJogadores);
                var jogadoresDTO = JogadorDTO.FromJogadores(jogadores);
                return Ok(jogadoresDTO);
            }
            catch (BaralhoNotFoundException ex)
            {
                return NotFound(new { erro = "Baralho não encontrado", detalhes = ex.Message });
            }
            catch (ExternalServiceUnavailableException ex)
            {
                return StatusCode(503, new { erro = "Serviço temporariamente indisponível", detalhes = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Ocorreu um erro interno no servidor" });
            }
        }

        [HttpPost("vencedor")]
        public async Task<ActionResult<JogadorDTO>> ObterVencedorAsync([FromBody] List<JogadorDTO> jogadoresDTO)
        {
            if (jogadoresDTO == null || jogadoresDTO.Count == 0)
            {
                return BadRequest(new { erro = "A lista de jogadores não pode estar vazia" });
            }

            try
            {
                var jogadores = JogadorDTO.ToJogadores(jogadoresDTO);
                var vencedor = await _jogoService.DeterminarVencedorAsync(jogadores);
                return Ok(new JogadorDTO(vencedor));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { erro = "Não foi possível determinar o vencedor", detalhes = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Ocorreu um erro interno no servidor" });
            }
        }

        [HttpPost("{baralhoId}/finalizar")]
        public async Task<ActionResult<bool>> FinalizarJogoAsync(string baralhoId)
        {
            if (string.IsNullOrEmpty(baralhoId))
            {
                return BadRequest(new { erro = "ID do baralho não pode ser nulo ou vazio" });
            }

            try
            {
                var resultado = await _jogoService.FinalizarJogoAsync(baralhoId);
                return Ok(resultado);
            }
            catch (BaralhoNotFoundException ex)
            {
                return NotFound(new { erro = "Baralho não encontrado", detalhes = ex.Message });
            }
            catch (ExternalServiceUnavailableException ex)
            {
                return StatusCode(503, new { erro = "Serviço temporariamente indisponível", detalhes = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Ocorreu um erro interno no servidor" });
            }
        }
    }
} 