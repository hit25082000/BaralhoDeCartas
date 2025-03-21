using BaralhoDeCartas.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaralhoDeCartas.Common
{
    public static class ValidacaoService
    {
        public static void ValidarNumeroJogadores(int numeroJogadores, int maximo = 10)
        {
            if (numeroJogadores <= 0)
            {
                throw new ArgumentException("O número de jogadores deve ser maior que zero");
            }

            if (numeroJogadores >= maximo)
            {
                throw new ArgumentException($"O número de jogadores deve ser menor que {maximo}");
            }
        }

        public static void ValidarBaralhoId(string baralhoId)
        {
            if (string.IsNullOrEmpty(baralhoId))
            {
                throw new ArgumentException("O ID do baralho não pode ser nulo ou vazio");
            }
        }

        public static void ValidarListaJogadores<T>(List<T> jogadores) where T : IJogador
        {
            if (jogadores == null || !jogadores.Any())
            {
                throw new ArgumentException("A lista de jogadores não pode estar vazia");
            }
        }

        public static void ValidarJogadoresDuplicados<T>(List<T> jogadores) where T : IJogador
        {
            var jogadoresAgrupados = jogadores.GroupBy(j => j.JogadorId);
            var jogadoresDuplicados = jogadoresAgrupados.Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            
            if (jogadoresDuplicados.Any())
            {
                throw new ArgumentException($"A lista de jogadores não pode ter jogadores com o mesmo Id. IDs duplicados: {string.Join(", ", jogadoresDuplicados)}");
            }
        }

        public static void ValidarCartasDuplicadas<T>(List<T> jogadores) where T : IJogador
        {
            var cartasRegistradas = new Dictionary<string, int>();
            
            foreach (var jogador in jogadores)
            {
                foreach (var carta in jogador.Cartas)
                {
                    string codigoCarta = carta.Codigo;
                    
                    if (cartasRegistradas.ContainsKey(codigoCarta))
                    {
                        int jogadorIdDuplicado = cartasRegistradas[codigoCarta];
                        throw new InvalidOperationException(
                            $"Carta duplicada encontrada: {codigoCarta}. " +
                            $"A carta está com o Jogador {jogadorIdDuplicado} e Jogador {jogador.JogadorId}");
                    }
                    
                    cartasRegistradas.Add(codigoCarta, jogador.JogadorId);
                }
            }
        }

        public static void ValidarQuantidadeCartasBaralho(IBaralho baralho, int minimoCartas)
        {
            if (baralho.QuantidadeDeCartasRestantes < minimoCartas)
            {
                throw new InvalidOperationException("Quantidade insuficiente de cartas no baralho");
            }
        }

        public static void ValidarJogadorDeBlackjack(IJogadorDeBlackjack jogador)
        {
            if (jogador == null)
            {
                throw new ArgumentNullException(nameof(jogador), "O jogador não pode ser nulo");
            }

            if (jogador.Parou || jogador.Estourou)
            {
                throw new InvalidOperationException($"O jogador {jogador.Nome} não pode comprar mais cartas.");
            }
        }
    }
} 