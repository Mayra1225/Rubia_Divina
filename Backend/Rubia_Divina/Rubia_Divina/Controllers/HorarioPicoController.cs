using Microsoft.AspNetCore.Mvc;
using Rubia_Divina.Services;

namespace Rubia_Divina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HorariosPicoController
    : ControllerBase
{
    private readonly HorarioPicoService _service;

    public HorariosPicoController(
        HorarioPicoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _service.ActualizarTablaAsync();

        var datos =
            await _service.ObtenerAsync();

        return Ok(datos);
    }

    [HttpPost("actualizar")]
    public async Task<IActionResult> Actualizar()
    {
        await _service.ActualizarTablaAsync();

        return Ok(new
        {
            mensaje =
                "Horarios pico recalculados."
        });
    }

    [HttpGet("analisis")]
    public async Task<IActionResult> Analisis()
    {
        return Ok(
            await _service
            .GenerarHorariosPicoAsync()
        );
    }
}