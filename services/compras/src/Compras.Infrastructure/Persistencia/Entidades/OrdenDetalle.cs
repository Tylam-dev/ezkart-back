using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compras.Infrastructure.Persistencia.Entidades;
[Table("orden_detalle")]
public class OrdenDetalle : EntidadBase
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Required]
    [Column("orden_id")]
    public Guid OrdenId { get; set; }
    public virtual Orden Orden { get; set; } = null!;
    [Required]
    [Column("producto_id")]
    public Guid ProductoId { get; set; }
    [Required]
    [Column("codigo_producto")]
    public string CodigoProducto { get; set; } = null!;
    [Required]
    [Column("nombre_producto")]
    public string NombreProducto { get; set; } = null!;
    [Required]
    [Column("precio_unitario")]
    [Precision(13,4)]
    public decimal PrecioUnitario { get; set; }
    [Required]
    [Column("cantidad")]
    public int Cantidad { get; set; }
    public class OrdenDetalleConfiguracion : EntidadBaseConfiguration<OrdenDetalle>
    {
        public override void Configure(EntityTypeBuilder<OrdenDetalle> builder)
        {
            base.Configure(builder);
        }
    }
}
