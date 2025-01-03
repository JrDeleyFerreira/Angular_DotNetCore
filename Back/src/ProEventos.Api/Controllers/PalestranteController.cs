using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application.Dtos;
using ProEventos.Persisttence.Pagination;

namespace ProEventos.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PalestrantesController : ControllerBase
{
    private readonly IPalestranteService _palestranteService;

    public PalestrantesController(IPalestranteService palestranteService)
        => _palestranteService = palestranteService;

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] PageParams pageParams)
    {
        try
        {
            var palestrantes = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);
            if (palestrantes == null) return NoContent();

            Response.AddPagination(palestrantes.CurrentPage,
                                   palestrantes.PageSize,
                                   palestrantes.TotalCount,
                                   palestrantes.TotalPages);

            return Ok(palestrantes);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
        }
    }

    [HttpGet()]
    public async Task<IActionResult> GetPalestrantes()
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(
                User.GetUserId(), true);

            return palestrante is null
                ? NoContent()
                : Ok(palestrante);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(PalestranteAddDto model)
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(
                User.GetUserId(), false);

            palestrante ??= await _palestranteService.AddPalestrantes(User.GetUserId(), model);

            return Ok(palestrante);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Put(PalestranteUpdateDto model)
    {
        try
        {
            var palestrante = await _palestranteService.UpdatePalestrante(User.GetUserId(), model);

            return palestrante is null
                ? NoContent()
                : Ok(palestrante);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
        }
    }
}
