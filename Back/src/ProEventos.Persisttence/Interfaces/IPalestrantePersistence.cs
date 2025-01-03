using ProEventos.Domain.Entities;
using ProEventos.Persisttence.Pagination;

namespace ProEventos.Persisttence.Interfaces;

public interface IPalestrantePersistence : IBasePersistence
{
    Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams,
        bool includeEventos = false);

    Task<Palestrante?> GetPalestranteByIdAsync(int userId, bool includeEventos = false);
}
