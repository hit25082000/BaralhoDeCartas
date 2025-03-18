namespace BaralhoDeCartas.Models.Interfaces
{
    public interface IJogadorDeBlackjack : IJogador
    {
        bool Estourou { get; }
        bool Parou { get; set; }
        bool TemBlackjack();
        int CalcularPontuacao();
    }
}
