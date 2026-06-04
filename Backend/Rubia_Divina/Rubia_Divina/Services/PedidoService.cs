using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.DTOs;
using Rubia_Divina.Models;

namespace Rubia_Divina.Services;

public class PedidoService
{
    private readonly AppDbContext _context;

    public PedidoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pedido>> ObtenerTodosAsync()
    {
        return await _context.Pedidos
            .Include(x => x.Detalles)
            .ThenInclude(x => x.Producto)
            .Include(x => x.Promocion)
            .OrderByDescending(x => x.FechaPedido)
            .ToListAsync();
    }

    public async Task<Pedido?> ObtenerUnoAsync(int id)
    {
        return await _context.Pedidos
            .Include(x => x.Detalles)
            .ThenInclude(x => x.Producto)
            .Include(x => x.Promocion)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Pedido> CrearAsync(PedidoDTO dto)
    {
        decimal total = 0;

        var detalles = new List<DetallePedido>();

        foreach (var item in dto.Detalles)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(x => x.Id == item.ProductoId);

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
            promo = await _context.Promociones
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.PromocionId &&
                    x.Activa);

            if (promo != null)
            {
                total -= total * (promo.Descuento / 100);
            }
        }

        var pedido = new Pedido
        {
            FechaPedido = DateTime.Now,
            PromocionId = promo?.Id,
            Total = total,
            Detalles = detalles
        };

        _context.Pedidos.Add(pedido);

        await _context.SaveChangesAsync();

        await RegistrarHorarioPico();

        return pedido;
    }

    private async Task RegistrarHorarioPico()
    {
        var ahora = DateTime.Now;

        string dia = ahora.DayOfWeek.ToString();

        var registro =
            await _context.HorariosPico
            .FirstOrDefaultAsync(x =>
                x.DiaSemana == dia &&
                x.Hora == ahora.Hour);

        if (registro == null)
        {
            registro = new HorarioPico
            {
                DiaSemana = dia,
                Hora = ahora.Hour,
                CantidadPedidos = 1,
                EsHorarioPico = false
            };

            _context.HorariosPico.Add(registro);
        }
        else
        {
            registro.CantidadPedidos++;
        }

        registro.EsHorarioPico =
            registro.CantidadPedidos >= 3;

        await _context.SaveChangesAsync();
    }

    public async Task<Pedido?> ActualizarAsync(
    int id,
    PedidoDTO dto)
    {
        var pedido =
            await _context.Pedidos
            .Include(x => x.Detalles)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (pedido == null)
            return null;

        _context.DetallePedidos.RemoveRange(
            pedido.Detalles
        );

        decimal total = 0;

        var nuevosDetalles =
            new List<DetallePedido>();

        foreach (var item in dto.Detalles)
        {
            var producto =
                await _context.Productos
                .FirstOrDefaultAsync(x =>
                    x.Id == item.ProductoId);

            if (producto == null)
                throw new Exception(
                    "Producto no encontrado"
                );

            var subtotal =
                producto.Precio * item.Cantidad;

            total += subtotal;

            nuevosDetalles.Add(
                new DetallePedido
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
            promo =
                await _context.Promociones
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.PromocionId &&
                    x.Activa);

            if (promo != null)
            {
                total -= total *
                    (promo.Descuento / 100);
            }
        }

        pedido.Total = total;
        pedido.PromocionId = promo?.Id;
        pedido.Detalles = nuevosDetalles;

        await _context.SaveChangesAsync();

        return pedido;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var pedido =
            await _context.Pedidos
            .Include(x => x.Detalles)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (pedido == null)
            return false;

        _context.DetallePedidos.RemoveRange(
            pedido.Detalles
        );

        _context.Pedidos.Remove(pedido);

        await _context.SaveChangesAsync();

        return true;
    }
}