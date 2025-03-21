using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;
using BaralhoDeCartas.Models.DTOs;
using BaralhoDeCartas.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BaralhoDeCartas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackjackApiController : ControllerBase
    {
        private readonly IBlackjackService _jogoService;

        public BlackjackApiController(IBlackjackService jogoService, ILogger<BlackjackApiController> logger)
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

        [HttpPost("{baralhoId}/iniciar-rodada/{numeroJogadores}")]
        public async Task<ActionResult<List<JogadorBlackjackDTO>>> IniciarRodadaAsync(string baralhoId, int numeroJogadores)
        {
            if (string.IsNullOrEmpty(baralhoId))
            {
                return BadRequest(new { erro = "O ID do baralho não pode ser nulo ou vazio" });
            }

            if (numeroJogadores <= 0)
            {
                return BadRequest(new { erro = "O número de jogadores deve ser maior que zero" });
            }

            try
            {
                var jogadores = await _jogoService.IniciarRodadaAsync(baralhoId, numeroJogadores);
                var jogadoresDTO = JogadorBlackjackDTO.FromJogadores(jogadores);
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

        [HttpPost("{baralhoId}/comprar-carta")]
        public async Task<ActionResult<CartaDTO>> ComprarCarta(string baralhoId, [FromBody] JogadorBlackjackDTO jogadorDTO)
        {
            if (string.IsNullOrEmpty(baralhoId))
            {
                return BadRequest(new { erro = "O ID do baralho não pode ser nulo ou vazio" });
            }

            if (jogadorDTO == null)
            {
                return BadRequest(new { erro = "O jogador não pode ser nulo" });
            }

            try
            {
                var jogadores = JogadorBlackjackDTO.ToJogadores(new List<JogadorBlackjackDTO> { jogadorDTO });
                var jogador = jogadores.First();
                var novaCarta = await _jogoService.ComprarCartaAsync(baralhoId, jogador);
                return Ok(new CartaDTO(novaCarta));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { erro = "Operação inválida", detalhes = ex.Message });
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

        [HttpPost("parar")]
        public ActionResult<JogadorBlackjackDTO> PararJogador([FromBody] JogadorBlackjackDTO jogadorDTO)
        {
            if (jogadorDTO == null)
            {
                return BadRequest(new { erro = "O jogador não pode ser nulo" });
            }

            try
            {
                var jogadores = JogadorBlackjackDTO.ToJogadores(new List<JogadorBlackjackDTO> { jogadorDTO });
                var jogador = jogadores.First();
                jogador.Parou = true;
                return Ok(new JogadorBlackjackDTO(jogador));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Ocorreu um erro interno no servidor" });
            }
        }

        [HttpPost("{baralhoId}/finalizar")]
        public async Task<ActionResult<ResultadoRodadaBlackjackDTO>> FinalizarRodadaAsync(string baralhoId, [FromBody] List<JogadorBlackjackDTO> jogadoresDTO)
        {
            if (string.IsNullOrEmpty(baralhoId))
            {
                return BadRequest(new { erro = "O ID do baralho não pode ser nulo ou vazio" });
            }

            if (jogadoresDTO == null || jogadoresDTO.Count == 0)
            {
                return BadRequest(new { erro = "A lista de jogadores não pode estar vazia" });
            }

            try
            {
                var jogadores = JogadorBlackjackDTO.ToJogadores(jogadoresDTO);
                var vencedores = _jogoService.DeterminarVencedoresAsync(jogadores);
                await _jogoService.RetornarCartasAoBaralhoAsync(baralhoId);

                var resultado = new ResultadoRodadaBlackjackDTO
                {
                    Vencedores = JogadorBlackjackDTO.FromJogadores(vencedores),
                    JogadoresFinais = jogadoresDTO
                };

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