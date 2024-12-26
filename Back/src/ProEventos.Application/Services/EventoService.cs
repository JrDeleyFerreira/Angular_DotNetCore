using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persisttence.Interfaces;

namespace ProEventos.Application.Services;

public class EventoService : IEventoService
{
    private readonly IBasePersistence _basePersistence;
    private readonly IEventoPersistence _eventoPersistence;
    private readonly IMapper _mapper;

    public EventoService(
        IBasePersistence basePersistence,
        IEventoPersistence eventoPersistence,
        IMapper mapper)
    {
        _basePersistence = basePersistence;
        _eventoPersistence = eventoPersistence;
        _mapper = mapper;
    }

    public async Task<EventoDto?> AddEventos(int userId, EventoDto model)
    {
        try
        {
            var evento = _mapper.Map<Evento>(model);
            evento.UserId = userId;

			_basePersistence.Add(evento);

            if (await _basePersistence.SaveChangesAsync())
            {
                var eventoRetorno = await _eventoPersistence.GetEventoByIdAsync(userId, evento.Id, false);

                return _mapper.Map<EventoDto>(eventoRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto?> UpdateEvento(int userId, int eventoId, EventoDto model)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(userId, eventoId, false);
            if (evento == null) return null;

            model.Id = evento.Id;
            model.UserId = userId;

			_mapper.Map(model, evento);

            _basePersistence.Update<Evento>(evento);

            if (await _basePersistence.SaveChangesAsync())
            {
                var eventoRetorno = await _eventoPersistence.GetEventoByIdAsync(evento.Id, false);

                return _mapper.Map<EventoDto>(eventoRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> DeleteEvento(int userId, int eventoId)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(userId, eventoId, false)
                ?? throw new Exception("Evento para delete não encontrado.");

            _basePersistence.Delete<Evento>(evento);
            return await _basePersistence.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]?> GetAllEventosAsync(int userId, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersistence.GetAllEventosAsync(userId, includePalestrantes);
            if (eventos == null) return null;

            var resultado = _mapper.Map<EventoDto[]>(eventos);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]?> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersistence.GetAllEventosByTemaAsync(userId, tema, includePalestrantes);
            if (eventos == null) return null;

            var resultado = _mapper.Map<EventoDto[]>(eventos);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto?> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(userId, eventoId, includePalestrantes);
            if (evento == null) return null;

            var resultado = _mapper.Map<EventoDto>(evento);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
