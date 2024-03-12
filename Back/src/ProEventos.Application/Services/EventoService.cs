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

    public async Task<EventoDto?> AddEventos(EventoDto model)
    {
        try
        {
            var evento = _mapper.Map<Evento>(model);

            _basePersistence.Add<Evento>(evento);

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

    public async Task<EventoDto?> UpdateEvento(int eventoId, EventoDto model)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(eventoId, false);
            if (evento == null) return null;

            model.Id = evento.Id;

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

    public async Task<bool> DeleteEvento(int eventoId)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(eventoId, false)
                ?? throw new Exception("Evento para delete não encontrado.");

            _basePersistence.Delete<Evento>(evento);
            return await _basePersistence.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]?> GetAllEventosAsync(bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersistence.GetAllEventosAsync(includePalestrantes);
            if (eventos == null) return null;

            var resultado = _mapper.Map<EventoDto[]>(eventos);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto[]?> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
        try
        {
            var eventos = await _eventoPersistence.GetAllEventosByTemaAsync(tema, includePalestrantes);
            if (eventos == null) return null;

            var resultado = _mapper.Map<EventoDto[]>(eventos);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<EventoDto?> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
        try
        {
            var evento = await _eventoPersistence.GetEventoByIdAsync(eventoId, includePalestrantes);
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
