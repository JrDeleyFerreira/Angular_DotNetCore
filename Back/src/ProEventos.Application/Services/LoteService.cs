using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;

namespace ProEventos.Application.Services;

public class LoteService : ILoteService
{
	public LoteService()
	{
		
	}

	public Task<bool> DeleteLote(int eventoId, int loteId) => throw new NotImplementedException();
	public Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId) => throw new NotImplementedException();
	public Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId) => throw new NotImplementedException();
	public Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models) => throw new NotImplementedException();
}
