using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compras.Infrastructure.Persistencia.Entidades;
[Table("carrito_item")]
public class CarritoItem : EntidadBase
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Required]
    [Column("carrito_id")]
    public Guid CarritoId { get; set; }
    public virtual Carrito Carrito { get; set; } = null!;
    [Required]
    [Column("producto_id")]
    public Guid ProductoId { get; set; }
    [Required]
    [Column("cantidad")]
    public int Cantidad { get; set; }
    public class CarritoItemConfiguracion : EntidadBaseConfiguration<CarritoItem>
    {
        public override void Configure(EntityTypeBuilder<CarritoItem> builder)
        {
            base.Configure(builder);
        }
    }
}
