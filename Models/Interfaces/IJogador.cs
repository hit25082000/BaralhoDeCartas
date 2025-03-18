namespace BaralhoDeCartas.Models.Interfaces
{
    public interface IJogador
    {
        int JogadorId { get; set; }
        string Nome { get; set; }
        List<ICarta> Cartas { get; set; }
        ICarta ObterCartaMaisAlta();
    }
}
