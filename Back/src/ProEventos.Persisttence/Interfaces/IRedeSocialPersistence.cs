using ProEventos.Domain.Entities;

namespace ProEventos.Persisttence.Interfaces;

public interface IRedeSocialPersistence : IBasePersistence
{
    Task<RedeSocial?> GetRedesSociaisByEventoIdsAsync(int eventoId, int id);
    Task<RedeSocial?> GetRedesSociaisByPalestranteIdsAsync(int palestranteId, int id);
    Task<RedeSocial[]?> GetAllByEventoIdAsync(int eventoId);
    Task<RedeSocial[]?> GetAllByPalestranteIdAsync(int palestranteId);
}
