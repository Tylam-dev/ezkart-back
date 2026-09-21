using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Compras.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compras.Infrastructure.Persistencia.Entidades;

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
    [Column("estado", TypeName = "char(1)")]
    public char Estado { get; set; } = (char)EstadoEnum.Activo;
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
    }
}
