namespace InventarioAPI.Models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int Stock { get; set; } = 0;
    public decimal Precio { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
