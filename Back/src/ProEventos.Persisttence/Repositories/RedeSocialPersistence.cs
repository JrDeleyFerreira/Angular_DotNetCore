using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Persisttence.Context;
using ProEventos.Persisttence.Interfaces;

namespace ProEventos.Persisttence.Repositories;

public class RedeSocialPersistence : BasePersistence, IRedeSocialPersistence
{
    private readonly ProEventosContext _context;

    public RedeSocialPersistence(ProEventosContext context) : base(context)
        => _context = context;


    public async Task<RedeSocial[]?> GetAllByEventoIdAsync(int eventoId)
    {
        IQueryable<RedeSocial> query = _context.RedeSocials;

        query = query.AsNoTracking()
            .Where(rs => rs.EventoId == eventoId);

        return await query.ToArrayAsync();
    }

    public async Task<RedeSocial[]?> GetAllByPalestranteIdAsync(int palestranteId)
    {
        IQueryable<RedeSocial> query = _context.RedeSocials;

        query = query.AsNoTracking()
            .Where(rs => rs.EventoId == palestranteId);

        return await query.ToArrayAsync();
    }

    public async Task<RedeSocial?> GetRedesSociaisByEventoIdsAsync(int eventoId, int id)
    {
        IQueryable<RedeSocial> query = _context.RedeSocials;
        query = query.AsNoTracking()
            .Where(rs =>
                rs.EventoId == eventoId &&
                rs.Id == id);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<RedeSocial?> GetRedesSociaisByPalestranteIdsAsync(int palestranteId, int id)
    {
        IQueryable<RedeSocial> query = _context.RedeSocials;

        query = query.AsNoTracking()
            .Where(rs =>
                rs.PalestranteId == palestranteId &&
                rs.Id == id);

        return await query.FirstOrDefaultAsync();
    }
}
