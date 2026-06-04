using Microsoft.AspNetCore.Mvc;
using Rubia_Divina.Services;

namespace Rubia_Divina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnaliticaController : ControllerBase
{
    private readonly AnaliticaService _service;

    public AnaliticaController(
        AnaliticaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(
            await _service.ObtenerAnalisisAsync()
        );
    }
}