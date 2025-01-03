using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Api.Utils;
using ProEventos.Application.Dtos;
using ProEventos.Persisttence.Pagination;

namespace ProEventos.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    private readonly IEventoService _eventoService;
    private readonly IAccountService _accountService;
    private readonly IUtil _util;
    private readonly string _folder = "Images";

    public EventoController(IEventoService eventoService, IAccountService accountService, IUtil util)
    {
        _eventoService = eventoService;
        _accountService = accountService;
        _util = util;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
    {
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, true);
            if (eventos is null) return NoContent();

            Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
            return evento is null
                ? NoContent()
                : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(EventoDto model)
    {
        try
        {
            var evento = await _eventoService.AddEventos(User.GetUserId(), model);
            return evento is null
                ? NoContent()
                : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, EventoDto model)
    {
        try
        {
            var evento = await _eventoService.UpdateEvento(User.GetUserId(), id, model);
            return evento is null
                ? NoContent()
                : Ok(evento);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
            if (evento == null) return NoContent();

            if (await _eventoService.DeleteEvento(User.GetUserId(), id))
            {
                _util.DeleteImage(_folder, evento.ImagemURL!);
                return Ok(new { message = "Deletado" });
            }
            else
            {
                throw new Exception("Ocorreu um problem não específico ao tentar deletar Evento.");
            }
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar eventos. Erro: {ex.Message}");
        }
    }

    [HttpPost("upload-image/{eventoId}")]
    public async Task<IActionResult> UploadImage(int eventoId)
    {
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
            if (evento == null) return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                _util.DeleteImage(_folder, evento.ImagemURL!);
                evento.ImagemURL = await _util.SaveImage(file, _folder);
            }
            var EventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);

            return Ok(EventoRetorno);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
        }
    }
}
