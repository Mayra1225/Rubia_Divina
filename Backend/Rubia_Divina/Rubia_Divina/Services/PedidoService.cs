using Rubia_Divina.DTOs;
using Rubia_Divina.FactoryMethods;
using Rubia_Divina.Interfaces.Repositories;
using Rubia_Divina.Interfaces.Services;
using Rubia_Divina.Models;

namespace Rubia_Divina.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _repository;
    private readonly IPedidoFactory _factory;
    private readonly IHorarioPicoService _horarioService;

    public PedidoService(
        IPedidoRepository repository,
        IPedidoFactory factory,
        IHorarioPicoService horarioService)
    {
        _repository = repository;
        _factory = factory;
        _horarioService = horarioService;
    }


    public async Task<List<Pedido>> ObtenerTodosAsync()
    {
        return await _repository.ObtenerTodosAsync();
    }


    public async Task<Pedido?> ObtenerUnoAsync(int id)
    {
        return await _repository.ObtenerUnoAsync(id);
    }


    public async Task<Pedido> CrearAsync(PedidoDTO dto)
    {
        decimal total = 0;

        var detalles = new List<DetallePedido>();

        foreach (var item in dto.Detalles)
        {
            var producto =
                await _repository.ObtenerProductoAsync(item.ProductoId);

            if (producto == null)
                throw new Exception("Producto no encontrado");

            if (producto.Stock < item.Cantidad)
                throw new Exception($"Stock insuficiente para {producto.Nombre}");

            producto.Stock -= item.Cantidad;

            var subtotal = producto.Precio * item.Cantidad;

            total += subtotal;

            detalles.Add(new DetallePedido
            {
                ProductoId = producto.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.Precio,
                Subtotal = subtotal
            });
        }

        Promocion? promo = null;

        if (dto.PromocionId.HasValue)
        {
            promo = await _repository.ObtenerPromocionAsync(dto.PromocionId.Value);

            if (promo != null)
            {
                total -= total * (promo.Descuento / 100);
            }
        }


        var pedido = _factory.Crear(
            total,
            promo?.Id,
            detalles
        );

        await _repository.AgregarPedidoAsync(pedido);

        await _repository.GuardarCambiosAsync();

        await RegistrarHorarioPico();

        return pedido;
    }


    public async Task<Pedido?> ActualizarAsync(
        int id,
        PedidoDTO dto)
    {
        var pedido =
            await _repository.ObtenerUnoAsync(id);

        if (pedido == null)
            return null;

        await _repository.EliminarDetallesAsync(pedido.Detalles);

        decimal total = 0;

        var nuevosDetalles = new List<DetallePedido>();

        foreach (var item in dto.Detalles)
        {
            var producto =
                await _repository.ObtenerProductoAsync(item.ProductoId);

            if (producto == null)
                throw new Exception("Producto no encontrado");

            var subtotal = producto.Precio * item.Cantidad;

            total += subtotal;

            nuevosDetalles.Add(new DetallePedido
            {
                ProductoId = producto.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.Precio,
                Subtotal = subtotal
            });
        }

        Promocion? promo = null;

        if (dto.PromocionId.HasValue)
        {
            promo = await _repository.ObtenerPromocionAsync(dto.PromocionId.Value);

            if (promo != null)
            {
                total -= total * (promo.Descuento / 100);
            }
        }

        pedido.Total = total;
        pedido.PromocionId = promo?.Id;
        pedido.Detalles = nuevosDetalles;

        await _repository.GuardarCambiosAsync();

        return pedido;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var pedido =
            await _repository.ObtenerUnoAsync(id);

        if (pedido == null)
            return false;

        await _repository.EliminarDetallesAsync(pedido.Detalles);
        await _repository.EliminarPedidoAsync(pedido);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private async Task RegistrarHorarioPico()
    {
        await _horarioService.ActualizarTablaAsync();
    }
}