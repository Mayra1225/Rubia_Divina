using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rubia_Divina.DTOs;
using Rubia_Divina.Interfaces.Services;
using System.Security.Claims;

namespace Rubia_Divina.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    private int ObtenerUsuarioId()
    {
        var claim = User.FindFirst("uid")?.Value;

        if (string.IsNullOrWhiteSpace(claim))
            throw new UnauthorizedAccessException(
                "No se encontró el usuario en el token.");

        return int.Parse(claim);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var productos =
            await _productoService.ObtenerPorUsuarioAsync(
                ObtenerUsuarioId());

        return Ok(productos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var producto =
            await _productoService.ObtenerUnoAsync(
                id,
                ObtenerUsuarioId());

        return producto is null
            ? NotFound()
            : Ok(producto);
    }

    [HttpGet("categoria/{categoriaId:int}")]
    public async Task<IActionResult> GetPorCategoria(int categoriaId)
    {
        var productos =
            await _productoService.ObtenerPorCategoriaAsync(
                categoriaId,
                ObtenerUsuarioId());

        return Ok(productos);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProductoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var producto =
            await _productoService.CrearAsync(
                dto,
                ObtenerUsuarioId());

        return CreatedAtAction(
            nameof(GetById),
            new { id = producto.Id },
            producto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, ProductoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var producto =
            await _productoService.ActualizarAsync(
                id,
                dto,
                ObtenerUsuarioId());

        return producto is null
            ? NotFound()
            : Ok(producto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado =
            await _productoService.EliminarAsync(
                id,
                ObtenerUsuarioId());

        return eliminado
            ? Ok(new { message = "Eliminado correctamente" })
            : NotFound();
    }
}