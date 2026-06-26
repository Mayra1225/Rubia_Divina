using Microsoft.AspNetCore.Mvc;
using Rubia_Divina.Interfaces.Services;

namespace Rubia_Divina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnaliticaController : ControllerBase
{
    private readonly IAnaliticaService _service;

    public AnaliticaController(IAnaliticaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.ObtenerAnalisisAsync());
    }
}