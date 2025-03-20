using BaralhoDeCartas.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BaralhoDeCartas.Models
{
    public class Jogador : IJogador
    {
        public Jogador(int jogadorId, string nome)
        {
            JogadorId = jogadorId;
            Nome = nome;
            Cartas = new List<ICarta>();
        }

        public int JogadorId { get; }
        public string Nome { get; }
        public List<ICarta> Cartas { get; }

        public void AdicionarCarta(ICarta carta)
        {
            Cartas.Add(carta);
        }

        public ICarta ObterCartaMaisAlta()
        {
            return Cartas.OrderByDescending(c => c.Valor).FirstOrDefault();
        }
    }
}
