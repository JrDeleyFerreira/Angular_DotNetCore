using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Persisttence.Context;
using ProEventos.Persisttence.Interfaces;

namespace ProEventos.Persisttence.Repositories;

public class PalestrantePersistence : IPalestrantePersistence
{
    private readonly ProEventosContext _context;

    public PalestrantePersistence(ProEventosContext context)
        => _context = context;

    public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
    {
        var query = IncludeCompositions(includeEventos);

        return await query.AsNoTracking().OrderBy(p => p.Id).ToArrayAsync();
    }

    public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
    {
        var query = IncludeCompositions(includeEventos);

        return await query.AsNoTracking()
            .OrderBy(p => p.Id)
            .Where(p => p.Nome!.ToLower().Contains(nome.ToLower()))
            .ToArrayAsync();
    }

    public async Task<Palestrante?> GetPalestranteByIdAsync(int palestranteId, bool includeEventos)
    {
        var query = IncludeCompositions(includeEventos);

        return await query.AsNoTracking()
            .OrderBy(p => p.Id)
            .FirstOrDefaultAsync(p => p.Id == palestranteId);
    }

    private IQueryable<Palestrante> IncludeCompositions(bool includeEventos)
    {
        IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedesSociais);

        if (includeEventos)
        {
            query = query
                .Include(p => p.PalestrantesEventos)!
                .ThenInclude(pe => pe.Evento);
        }

        return query;
    }
}
