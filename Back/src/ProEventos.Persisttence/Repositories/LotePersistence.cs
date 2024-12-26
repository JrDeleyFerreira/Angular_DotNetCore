using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Persisttence.Context;
using ProEventos.Persisttence.Interfaces;

namespace ProEventos.Persisttence.Repositories;

public class LotePersistence : ILotePersistence
{
	private readonly ProEventosContext _context;

	public LotePersistence(ProEventosContext context)
		=> _context = context;
	

	public async Task<Lote?> GetLoteByIdsAsync(int eventoId, int id)
	{
		IQueryable<Lote> query = _context.Lotes;

		query = query.AsNoTracking()
					 .Where(lote => lote.EventoId == eventoId
								 && lote.Id == id);

		return await query.FirstOrDefaultAsync();
	}

	public async Task<Lote[]?> GetLotesByEventoIdAsync(int eventoId)
	{
		IQueryable<Lote> query = _context.Lotes;

		query = query.AsNoTracking()
					 .Where(lote => lote.EventoId == eventoId);

		return await query.ToArrayAsync();
	}
}
