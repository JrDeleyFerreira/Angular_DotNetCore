using ProEventos.Domain.Entities;

namespace ProEventos.Persisttence.Interfaces;

public interface IEventoPersistence
{
    Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
    Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
    Task<Evento?> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);
}
