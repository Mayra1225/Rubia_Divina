using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rubia_Divina.DTOs;
using Rubia_Divina.Services;
using System.Security.Claims;

namespace Rubia_Divina.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ProductoService _productoService;

    public ProductosController(ProductoService productoService)
    {
        _productoService = productoService;
    }

    private int ObtenerUsuarioId()
    {
        var claim = User.FindFirst("uid")?.Value;

        if (string.IsNullOrWhiteSpace(claim))
        {
            throw new UnauthorizedAccessException("No se encontró el identificador del usuario en el token.");
        }

        return int.Parse(claim);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var productos = await _productoService.ObtenerPorUsuarioAsync(ObtenerUsuarioId());
        return Ok(productos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _productoService.ObtenerUnoAsync(id, ObtenerUsuarioId());
        return producto is null
            ? NotFound(new { message = "Producto no encontrado." })
            : Ok(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProductoDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var producto = await _productoService.CrearAsync(dto, ObtenerUsuarioId());
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProductoDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var producto = await _productoService.ActualizarAsync(id, dto, ObtenerUsuarioId());
        return producto is null
            ? NotFound(new { message = "Producto no encontrado." })
            : Ok(producto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _productoService.EliminarAsync(id, ObtenerUsuarioId());
        return !eliminado
            ? NotFound(new { message = "Producto no encontrado." })
            : Ok(new { message = "Producto eliminado correctamente." });
    }
}