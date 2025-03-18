namespace BaralhoDeCartas.Models
{
    public class Jogador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Carta> Cartas { get; set; }
        public bool Parou { get; set; }
        public bool Estourou => CalcularPontuacao() > 21;

        public Jogador()
        {
            Cartas = new List<Carta>();
            Parou = false;
        }

        public int CalcularPontuacao()
        {
            var ases = Cartas.Count(c => c.Value == "ACE");
            var pontuacao = Cartas.Sum(c => c.ValorBlackjack);

            // Ajusta o valor dos ases (11 -> 1) se necessário para não estourar
            while (pontuacao > 21 && ases > 0)
            {
                pontuacao -= 10; // Converte um Ás de 11 para 1
                ases--;
            }

            return pontuacao;
        }

        public bool TemBlackjack()
        {
            return Cartas.Count == 2 && CalcularPontuacao() == 21;
        }

        public Carta ObterCartaMaisAlta()
        {
            return Cartas.OrderByDescending(c => c.ValorNumerico).FirstOrDefault();
        }
    }
}
