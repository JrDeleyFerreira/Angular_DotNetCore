using ProEventos.Domain.Entities;
using ProEventos.Persisttence.Pagination;

namespace ProEventos.Persisttence.Interfaces;

public interface IEventoPersistence
{
    Task<PageList<Evento>> GetAllEventosAsync(
        int userId, PageParams pageParams, bool includePalestrantes = false);

    Task<Evento?> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
}
