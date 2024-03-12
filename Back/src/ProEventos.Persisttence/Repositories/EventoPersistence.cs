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

    public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
    {
        var query = IncludeCompositions(includePalestrantes);

        return await query.AsNoTracking().OrderBy(e => e.Id).ToArrayAsync();
    }

    public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
        var query = IncludeCompositions(includePalestrantes);

        return await query.AsNoTracking()
            .OrderBy(e => e.Id)
            .Where(e => e.Tema!.ToLower().Contains(tema.ToLower()))
            .ToArrayAsync();
    }

    public async Task<Evento?> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
        var query = IncludeCompositions(includePalestrantes);

        return await query.AsNoTracking()
            .OrderBy(e => e.Id)
            .FirstOrDefaultAsync(e => e.Id == eventoId);
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
