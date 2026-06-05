using Microsoft.AspNetCore.Mvc;
using Rubia_Divina.Services;

namespace Rubia_Divina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _service;

    public DashboardController(
        DashboardService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(
            await _service.ObtenerResumenAsync()
        );
    }

    [HttpGet("producto-mas-vendido-dia")]
    public async Task<IActionResult>
    GetProductoMasVendidoDia()
    {
        return Ok(
            await _service.ObtenerProdcutoMasVendidoPorDia()
        );
    }
}