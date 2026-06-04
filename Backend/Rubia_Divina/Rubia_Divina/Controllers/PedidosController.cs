using Microsoft.AspNetCore.Mvc;
using Rubia_Divina.DTOs;
using Rubia_Divina.Services;

namespace Rubia_Divina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly PedidoService _service;

    public PedidosController(PedidoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.ObtenerTodosAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var pedido = await _service.ObtenerUnoAsync(id);

        if (pedido == null)
            return NotFound();

        return Ok(pedido);
    }

    [HttpPost]
    public async Task<IActionResult> Post(PedidoDTO dto)
    {
        return Ok(await _service.CrearAsync(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(
        int id,
        PedidoDTO dto)
    {
        var pedido =
            await _service.ActualizarAsync(id, dto);

        if (pedido == null)
            return NotFound();

        return Ok(pedido);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado =
            await _service.EliminarAsync(id);

        if (!eliminado)
            return NotFound();

        return Ok();
    }
}