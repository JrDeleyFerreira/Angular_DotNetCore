using ProEventos.Domain.Entities;

namespace ProEventos.Persisttence.Interfaces;

public interface IEventoPersistence
{
    Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
    Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
    Task<Evento?> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);
}
