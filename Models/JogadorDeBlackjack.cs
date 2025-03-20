using BaralhoDeCartas.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BaralhoDeCartas.Models
{
    public class JogadorDeBlackjack : Jogador, IJogadorDeBlackjack
    {
        public JogadorDeBlackjack(int jogadorId, string nome)
            : base(jogadorId, nome)
        {
            Parou = false;
        }

        public bool Parou { get; set; }

        public bool Estourou => CalcularPontuacao() > 21;

        public bool TemBlackjack()
        {
            return Cartas.Count == 2 && CalcularPontuacao() == 21;
        }

        public int CalcularPontuacao()
        {
            var cartasBlackjack = Cartas.Select(c => new CartaBlackjack(c)).ToList();
            int pontuacao = cartasBlackjack.Sum(c => c.ValorBlackjack);
            int ases = cartasBlackjack.Count(c => c.ValorSimbolico == "ACE");

            while (pontuacao > 21 && ases > 0)
            {
                pontuacao -= 10;
                ases--;
            }

            return pontuacao;
        }
    }
}

