using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Compras.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compras.Infrastructure.Persistencia.Entidades;
[Table("orden")]
public class Orden : EntidadBase
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Required]
    [Column("usuario_id")]
    public Guid UsuarioId { get; set; }
    [Required]
    [Column("estado_orden")]
    public EstadoOrdenEnum EstadoOrden { get; set; }
    [Required]
    [Column("total")]
    [Precision(13,4)]
    public decimal Total { get; set; }
    [Column("descuento_temporada")]
    public Guid? DescuentoTemporadaId { get; set; }
    public virtual DescuentoTemporada? DescuentoTemporada { get; set; }
    public virtual ICollection<OrdenDetalle> Detalles { get; set; } = new List<OrdenDetalle>();
    public class OrdenConfiguracion : EntidadBaseConfiguration<Orden>
    {
        public override void Configure(EntityTypeBuilder<Orden> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.UsuarioId);

            builder.HasMany(x => x.Detalles)
                .WithOne(x => x.Orden)
                .HasForeignKey(x => x.OrdenId);
        }
    }
}
