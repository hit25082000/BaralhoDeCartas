using Microsoft.AspNetCore.Mvc;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;
using BaralhoDeCartas.Models.DTOs;

namespace BaralhoDeCartas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackjackApiController : ControllerBase
    {
        private readonly IBlackjackService _jogoService;

        public BlackjackApiController(IBlackjackService jogoService)
        {
            _jogoService = jogoService;
        }

        [HttpGet("iniciar")]
        public async Task<ActionResult<IBaralho>> IniciarJogo()
        {
            try
            {
                var baralho = await _jogoService.IniciarJogo();
                return Ok(baralho);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar o jogo: {ex.Message}");
            }
        }

        [HttpPost("{baralhoId}/iniciar-rodada/{numeroJogadores}")]
        public async Task<ActionResult<List<JogadorBlackjackDTO>>> IniciarRodada(string baralhoId, int numeroJogadores)
        {
            try
            {
                if (numeroJogadores <= 0)
                {
                    return BadRequest("O número de jogadores deve ser maior que zero.");
                }

                var jogadores = await _jogoService.IniciarRodada(baralhoId, numeroJogadores);
                var jogadoresDTO = JogadorBlackjackDTO.FromJogadores(jogadores);
                return Ok(jogadoresDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar a rodada: {ex.Message}");
            }
        }

        [HttpPost("{baralhoId}/comprar-carta")]
        public async Task<ActionResult<CartaDTO>> ComprarCarta(string baralhoId, [FromBody] JogadorBlackjackDTO jogadorDTO)
        {
            try
            {
                var jogadores = JogadorBlackjackDTO.ToJogadores(new List<JogadorBlackjackDTO> { jogadorDTO });
                var jogador = jogadores.First();

                var novaCarta = await _jogoService.ComprarCarta(baralhoId, jogador);
                return Ok(new CartaDTO(novaCarta));
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

        [HttpPost("parar")]
        public ActionResult<JogadorBlackjackDTO> PararJogador([FromBody] JogadorBlackjackDTO jogadorDTO)
        {
            try
            {
                var jogadores = JogadorBlackjackDTO.ToJogadores(new List<JogadorBlackjackDTO> { jogadorDTO });
                var jogador = jogadores.First();
                jogador.Parou = true;

                return Ok(new JogadorBlackjackDTO(jogador));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao parar jogador: {ex.Message}");
            }
        }

        [HttpPost("{baralhoId}/finalizar")]
        public async Task<ActionResult<ResultadoRodadaBlackjackDTO>> FinalizarRodada(string baralhoId, [FromBody] List<JogadorBlackjackDTO> jogadoresDTO)
        {
            try
            {
                var jogadores = JogadorBlackjackDTO.ToJogadores(jogadoresDTO);
                var vencedores = _jogoService.DeterminarVencedores(jogadores);
                await _jogoService.FinalizarJogo(baralhoId);

                var resultado = new ResultadoRodadaBlackjackDTO
                {
                    Vencedores = JogadorBlackjackDTO.FromJogadores(vencedores),
                    JogadoresFinais = jogadoresDTO
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao finalizar rodada: {ex.Message}");
            }
        }
    }
} 