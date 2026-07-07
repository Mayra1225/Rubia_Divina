using Rubia_Divina.DTOs;
using Rubia_Divina.FactoryMethods;
using Rubia_Divina.Interfaces.Repositories;
using Rubia_Divina.Interfaces.Services;
using Rubia_Divina.Models;

namespace Rubia_Divina.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;
    private readonly IProductoFactory _factory;

    public ProductoService(
        IProductoRepository repository,
        IProductoFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<List<Producto>> ObtenerPorUsuarioAsync(
        int usuarioId)
    {
        return await _repository
            .ObtenerPorUsuarioAsync(usuarioId);
    }

    public async Task<Producto?> ObtenerUnoAsync(
        int id,
        int usuarioId)
    {
        return await _repository
            .ObtenerUnoAsync(id, usuarioId);
    }

    public async Task<Producto> CrearAsync(
        ProductoDTO dto,
        int usuarioId)
    {
        var categoria =
            await _repository.ObtenerCategoriaAsync(dto.CategoriaId);

        if (categoria == null)
        {
            throw new Exception(
                "La categoría no existe.");
        }

        var producto =
            _factory.Crear(dto, usuarioId);

        await _repository.AgregarAsync(producto);

        await _repository.GuardarCambiosAsync();

        return await _repository.ObtenerUnoAsync(
            producto.Id,
            usuarioId) ?? producto;
    }

    public async Task<Producto?> ActualizarAsync(
        int id,
        ProductoDTO dto,
        int usuarioId)
    {
        var producto =
            await _repository.ObtenerUnoAsync(
                id,
                usuarioId);

        if (producto == null)
            return null;

        var categoria =
            await _repository.ObtenerCategoriaAsync(
                dto.CategoriaId);

        if (categoria == null)
        {
            throw new Exception(
                "La categoría no existe.");
        }

        producto.Nombre = dto.Nombre.Trim();

        producto.Descripcion =
            dto.Descripcion.Trim();

        producto.Precio = dto.Precio;

        producto.Stock = dto.Stock;

        producto.CategoriaId =
            dto.CategoriaId;

        producto.ImagenUrl =
            dto.ImagenUrl;

        await _repository.GuardarCambiosAsync();

        return await _repository.ObtenerUnoAsync(
            producto.Id,
            usuarioId);
    }

    public async Task<bool> EliminarAsync(
        int id,
        int usuarioId)
    {
        var producto =
            await _repository.ObtenerUnoAsync(
                id,
                usuarioId);

        if (producto == null)
            return false;

        await _repository.EliminarAsync(producto);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<List<Producto>> ObtenerPorCategoriaAsync(
    int categoriaId,
    int usuarioId)
    {
        return await _repository.ObtenerPorCategoriaAsync(
            categoriaId,
            usuarioId);
    }
}