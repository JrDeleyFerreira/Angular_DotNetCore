using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persisttence.Interfaces;

namespace ProEventos.Application.Services;

public class RedeSocialService : IRedeSocialService
{
    private readonly IRedeSocialPersistence _redeSocialPersistence;
    private readonly IMapper _mapper;

    public RedeSocialService(
        IRedeSocialPersistence redeSocialPersistence,
        IMapper mapper)
    {
        _redeSocialPersistence = redeSocialPersistence;
        _mapper = mapper;
    }

    private async Task AddRedeSocial(int Id, RedeSocialDto model, bool isEvento)
    {
        try
        {
            var redeSocial = _mapper.Map<RedeSocial>(model);
            if (isEvento)
            {
                redeSocial.EventoId = Id;
                redeSocial.PalestranteId = null;
            }
            else
            {
                redeSocial.EventoId = null;
                redeSocial.PalestranteId = Id;
            }

            _redeSocialPersistence.Add(redeSocial);

            await _redeSocialPersistence.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<RedeSocialDto[]?> SaveByEvento(int eventoId, RedeSocialDto[] models)
    {
        try
        {
            var redeSocials = await _redeSocialPersistence.GetAllByEventoIdAsync(eventoId);
            if (redeSocials == null) return null;

            foreach (var model in models)
            {
                if (model.Id == 0)
                    await AddRedeSocial(eventoId, model, true);
                else
                {
                    var redeSocial = redeSocials.FirstOrDefault(rs => rs.Id == model.Id);
                    model.EventoId = eventoId;

                    _mapper.Map(model, redeSocial);
                    _redeSocialPersistence.Update(redeSocial!);

                    await _redeSocialPersistence.SaveChangesAsync();
                }
            }

            var redeSocialRetorno = await _redeSocialPersistence.GetAllByEventoIdAsync(eventoId);

            return _mapper.Map<RedeSocialDto[]>(redeSocialRetorno);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<RedeSocialDto[]?> SaveByPalestrante(int palestranteId, RedeSocialDto[] models)
    {
        try
        {
            var redeSocials = await _redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);
            if (redeSocials == null) return null;

            foreach (var model in models)
            {
                if (model.Id == 0)
                    await AddRedeSocial(palestranteId, model, false);
                else
                {
                    var redeSocial = redeSocials.FirstOrDefault(rs => rs.Id == model.Id);
                    model.PalestranteId = palestranteId;

                    _mapper.Map(model, redeSocial);
                    _redeSocialPersistence.Update(redeSocial!);

                    await _redeSocialPersistence.SaveChangesAsync();
                }
            }

            var RedeSocialRetorno = await _redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);

            return _mapper.Map<RedeSocialDto[]>(RedeSocialRetorno);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
    {
        try
        {
            var redeSocial =
                await _redeSocialPersistence.GetRedesSociaisByEventoIdsAsync(eventoId, redeSocialId)
                ?? throw new Exception("Rede Social por Evento para delete não encontrado.");

            _redeSocialPersistence.Delete(redeSocial);
            return await _redeSocialPersistence.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
    {
        try
        {
            var redeSocial = await
                _redeSocialPersistence.GetRedesSociaisByPalestranteIdsAsync(palestranteId, redeSocialId)
                ?? throw new Exception("Rede Social por Palestrante para delete não encontrado.");

            _redeSocialPersistence.Delete(redeSocial);
            return await _redeSocialPersistence.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<RedeSocialDto[]?> GetAllByEventoIdAsync(int eventoId)
    {
        try
        {
            var redeSocials = await _redeSocialPersistence.GetAllByEventoIdAsync(eventoId);
            if (redeSocials == null) return null;

            var resultado = _mapper.Map<RedeSocialDto[]>(redeSocials);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<RedeSocialDto[]?> GetAllByPalestranteIdAsync(int palestranteId)
    {
        try
        {
            var RedeSocials = await _redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);
            if (RedeSocials == null) return null;

            var resultado = _mapper.Map<RedeSocialDto[]>(RedeSocials);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<RedeSocialDto?> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
    {
        try
        {
            var redeSocial = await _redeSocialPersistence.GetRedesSociaisByEventoIdsAsync(
                eventoId, redeSocialId);
            if (redeSocial == null) return null;

            var resultado = _mapper.Map<RedeSocialDto>(redeSocial);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<RedeSocialDto?> GetRedeSocialPalestranteByIdsAsync(
        int palestranteId, int redeSocialId)
    {
        try
        {
            var redeSocial = await _redeSocialPersistence.GetRedesSociaisByPalestranteIdsAsync(
                palestranteId, redeSocialId);
            if (redeSocial == null) return null;

            var resultado = _mapper.Map<RedeSocialDto>(redeSocial);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
