using Microsoft.AspNetCore.Mvc;
using Rubia_Divina.DTOs;
using Rubia_Divina.Services;

namespace Rubia_Divina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromocionesController : ControllerBase
{
    private readonly PromocionService _service;

    public PromocionesController(PromocionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.ObtenerAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        PromocionDTO dto)
    {
        return Ok(await _service.CrearAsync(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(
        int id,
        PromocionDTO dto)
    {
        return Ok(await _service.ActualizarAsync(id, dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await _service.EliminarAsync(id));
    }
}