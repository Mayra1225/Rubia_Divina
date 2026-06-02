using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;

namespace Rubia_Divina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(
        AppDbContext context
    )
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categorias = await _context.Categorias
            .Select(c => new
            {
                c.Id,
                c.Nombre
            })
            .ToListAsync();

        return Ok(categorias);
    }
}