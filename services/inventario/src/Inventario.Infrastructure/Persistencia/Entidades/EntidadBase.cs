using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Inventario.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistencia.Entidades;

public abstract class EntidadBase
{
    [Required]
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; }
    [Column("fecha_actualizacion")]
    public DateTime? FechaActualizacion { get; set; }
    [Column("fecha_eliminacion")]
    public DateTime? FechaEliminacion { get; set; }
    [Required]
    [Column("estado")]
    public EstadoEnum Estado { get; set; }
}
    public abstract class EntidadBaseConfiguration<T>
    : IEntityTypeConfiguration<T>
    where T : EntidadBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.FechaCreacion)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.Property(x => x.Estado)
            .HasDefaultValue(EstadoEnum.Activo.ToString());
    }
}