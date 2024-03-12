using ProEventos.Domain.Entities;

namespace ProEventos.Persisttence.Interfaces;

public interface IPalestrantePersistence
{
    Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos);
    Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos);
    Task<Palestrante?> GetPalestranteByIdAsync(int palestranteId, bool includeEventos);
}
