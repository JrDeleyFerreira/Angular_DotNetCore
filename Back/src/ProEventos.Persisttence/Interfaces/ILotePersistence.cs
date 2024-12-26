using ProEventos.Domain.Entities;

namespace ProEventos.Persisttence.Interfaces;

public interface ILotePersistence
{
	/// <summary>
	/// Método get que retornará uma lista de lotes por eventoId. 
	/// </summary>
	/// <param name="eventoId">Código chave da tabela Evento</param>
	/// <returns>Array de Lotes</returns>
	Task<Lote[]?> GetLotesByEventoIdAsync(int eventoId);

	/// <summary>
	/// Método get que retornará apenas 1 Lote
	/// </summary>
	/// <param name="eventoId">Código chave da tabela Evento</param>
	/// <param name="id">Código chave da tabela Lote</param>
	/// <returns>Apenas 1 lote</returns>
	Task<Lote?> GetLoteByIdsAsync(int eventoId, int id);
}
