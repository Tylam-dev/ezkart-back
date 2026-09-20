using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compras.Infrastructure.Persistencia.Entidades;
[Table("descuento_temporada")]
public class DescuentoTemporada : EntidadBase
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Required]
    [Column("nombre")]
    public string Nombre { get; set; } = null!;
    [Required]
    [Column("porcentaje")]
    [Precision(5,2)]
    public decimal Porcentaje { get; set; }
    [Required]
    [Column("fecha_desde")]
    public DateTime FechaDesde { get; set; }
    [Required]
    [Column("fecha_hasta")]
    public DateTime FechaHasta { get; set; }
    public virtual ICollection<Orden> Ordenes { get; set; } = new List<Orden>();
    public class DescuentoTemporadaConfiguracion : EntidadBaseConfiguration<DescuentoTemporada>
    {
        public override void Configure(EntityTypeBuilder<DescuentoTemporada> builder)
        {
            base.Configure(builder);

            builder.HasMany(x => x.Ordenes)
                .WithOne(x => x.DescuentoTemporada)
                .HasForeignKey(x => x.DescuentoTemporadaId);
        }
    }
}
