using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Persisttence.Context;
using ProEventos.Persisttence.Interfaces;

namespace ProEventos.Persisttence.Repositories;

public class EventoPersistence : IEventoPersistence
{
    private readonly ProEventosContext _context;

    public EventoPersistence(ProEventosContext context)
        => _context = context;

    public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
    {
        var query = IncludeCompositions(includePalestrantes);

        return await query.AsNoTracking()
            .Where(e => e.UserId == userId)
			.OrderBy(e => e.Id).ToArrayAsync();
    }

    public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
    {
        var query = IncludeCompositions(includePalestrantes);

        return await query.AsNoTracking()
            .OrderBy(e => e.Id)
            .Where(e => 
                e.Tema!.Contains(tema, StringComparison.CurrentCultureIgnoreCase) && 
                e.UserId == userId)
            .ToArrayAsync();
    }

    public async Task<Evento?> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
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
