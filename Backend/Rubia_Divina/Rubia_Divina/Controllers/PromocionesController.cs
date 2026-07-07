using Microsoft.AspNetCore.Mvc;
using Rubia_Divina.DTOs;
using Rubia_Divina.Interfaces.Services;

namespace Rubia_Divina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromocionesController : ControllerBase
{
    private readonly IPromocionService _service;

    public PromocionesController(IPromocionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.ObtenerAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Post(PromocionDTO dto)
    {
        return Ok(await _service.CrearAsync(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, PromocionDTO dto)
    {
        var promo = await _service.ActualizarAsync(id, dto);

        if (promo == null)
            return NotFound();

        return Ok(promo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _service.EliminarAsync(id);

        if (!eliminado)
            return NotFound();

        return Ok();
    }
}