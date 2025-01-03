using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Entities;
using ProEventos.Persisttence.Interfaces;
using ProEventos.Persisttence.Pagination;

namespace ProEventos.Application.Services;

public class PalestranteService : IPalestranteService
{
    private readonly IPalestrantePersistence _palestrantePersist;
    private readonly IMapper _mapper;
    public PalestranteService(IPalestrantePersistence palestrantePersist, IMapper mapper)
    {
        _palestrantePersist = palestrantePersist;
        _mapper = mapper;
    }

    public async Task<PalestranteDto?> AddPalestrantes(int userId, PalestranteAddDto model)
    {
        try
        {
            var palestrante = _mapper.Map<Palestrante>(model);
            palestrante.UserId = userId;

            _palestrantePersist.Add(palestrante);

            if (await _palestrantePersist.SaveChangesAsync())
            {
                var PalestranteRetorno =
                    await _palestrantePersist.GetPalestranteByIdAsync(userId, false);

                return _mapper.Map<PalestranteDto>(PalestranteRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PalestranteDto?> UpdatePalestrante(int userId, PalestranteUpdateDto model)
    {
        try
        {
            var palestrante = await _palestrantePersist.GetPalestranteByIdAsync(userId, false);
            if (palestrante == null) return null;

            model.Id = palestrante.Id;
            model.UserId = userId;

            _mapper.Map(model, palestrante);

            _palestrantePersist.Update<Palestrante>(palestrante);

            if (await _palestrantePersist.SaveChangesAsync())
            {
                var palestranteRetorno = await _palestrantePersist.GetPalestranteByIdAsync(userId, false);

                return _mapper.Map<PalestranteDto>(palestranteRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PageList<PalestranteDto>?> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
    {
        try
        {
            var Palestrantes = await _palestrantePersist.GetAllPalestrantesAsync(
                pageParams, includeEventos);

            if (Palestrantes == null) return null;

            var resultado = _mapper.Map<PageList<PalestranteDto>>(Palestrantes);

            resultado.CurrentPage = Palestrantes.CurrentPage;
            resultado.TotalPages = Palestrantes.TotalPages;
            resultado.PageSize = Palestrantes.PageSize;
            resultado.TotalCount = Palestrantes.TotalCount;

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PalestranteDto?> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
    {
        try
        {
            var Palestrante = await _palestrantePersist.GetPalestranteByIdAsync(userId, includeEventos);
            if (Palestrante == null) return null;

            var resultado = _mapper.Map<PalestranteDto>(Palestrante);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
