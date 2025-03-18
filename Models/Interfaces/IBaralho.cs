namespace BaralhoDeCartas.Models.Interfaces
{
    public interface IBaralho
    {
        string BaralhoId { get; set; }
        bool FoiEmbaralhado { get; set; }
        int QuantidadeDeCartasRestantes { get; set; }
    }
}
