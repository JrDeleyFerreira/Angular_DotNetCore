using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Domain.Enums;
using ProEventos.Persisttence.Context;
using ProEventos.Persisttence.Interfaces;
using ProEventos.Persisttence.Pagination;

namespace ProEventos.Persisttence.Repositories;

public class PalestrantePersistence : BasePersistence, IPalestrantePersistence
{
    private readonly ProEventosContext _context;

    public PalestrantePersistence(ProEventosContext context) : base(context)
        => _context = context;

    public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams,
        bool includeEventos = false)
    {
        var query = IncludeCompositions(includeEventos);

        query = query.AsNoTracking()
            .Where(p =>
                p.MiniCurriculo!.ToLower().Contains(pageParams.Terms!.ToLower()) ||
                p.User!.FirstName!.ToLower().Contains(pageParams.Terms.ToLower()) ||
                p.User!.LastName!.ToLower().Contains(pageParams.Terms.ToLower()) &&
                p.User!.Function == Funcao.Palestrante)
            .OrderBy(p => p.Id);

        return await PageList<Palestrante>.CreateAsync
            (query, pageParams.PageNumber, pageParams.PageSize);
    }

    public async Task<Palestrante?> GetPalestranteByIdAsync(int userId, bool includeEventos)
    {
        var query = IncludeCompositions(includeEventos);

        return await query.AsNoTracking()
            .OrderBy(p => p.Id)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    private IQueryable<Palestrante> IncludeCompositions(bool includeEventos)
    {
        IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.User)
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
