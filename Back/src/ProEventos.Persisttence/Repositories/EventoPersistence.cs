using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Persisttence.Context;
using ProEventos.Persisttence.Interfaces;
using ProEventos.Persisttence.Pagination;

namespace ProEventos.Persisttence.Repositories;

public class EventoPersistence : IEventoPersistence
{
    private readonly ProEventosContext _context;

    public EventoPersistence(ProEventosContext context)
        => _context = context;

    public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams,
        bool includePalestrantes = false)
    {
        var query = IncludeCompositions(includePalestrantes);

        query = query.AsNoTracking()
        .Where(e =>
            (e.Tema!.Contains(pageParams.Terms!, StringComparison.InvariantCultureIgnoreCase) ||
             e.Local!.Contains(pageParams.Terms!, StringComparison.InvariantCultureIgnoreCase)) &&
            e.UserId == userId)
        .OrderBy(e => e.Id);

        return await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
    }

    public async Task<Evento?> GetEventoByIdAsync(int userId, int eventoId,
        bool includePalestrantes = false)
    {
        var query = IncludeCompositions(includePalestrantes);

        return await query.AsNoTracking()
            .OrderBy(e => e.Id)
            .FirstOrDefaultAsync(e => e.Id == eventoId && e.UserId == userId);
    }

    private IQueryable<Evento> IncludeCompositions(bool includePalestrantes)
    {
        IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

        if (includePalestrantes)
        {
            query = query
                .Include(e => e.PalestrantesEventos)!
                .ThenInclude(pe => pe.Palestrante);
        }

        return query;
    }
}
