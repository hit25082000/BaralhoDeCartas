using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Api.Interfaces
{
    public interface IApiService<T>
    {
        Task CreateAsync(T obj);
        Task<IEnumerable<T>?> ListAsync();
    }
} 