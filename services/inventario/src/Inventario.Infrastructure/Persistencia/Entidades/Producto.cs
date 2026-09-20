using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistencia.Entidades;
[Table("producto")]
public class Producto : EntidadBase
{
    [Key]
    public Guid Id {get;set;}
    [Required]
    [Column("codigo")]
    public string Codigo {get;set;}
    [Required]
    [Column("nombre")]
    public string Nombre {get;set;}
    [Required]
    [Column("precio")]
    [Precision(13,4)]
    public decimal Precio {get;set;}
    [Required]
    [Column("existencia")]
    public int Existencia {get;set;}
    public uint Version {get;set;}
    public class UsuarioConfiguracion : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        { 
            builder.Property(x => x.Precio)
                .HasDefaultValue(0);

            builder.Property(x => x.Existencia)
                .HasDefaultValue(0);

            builder.Property(p => p.Version)
            .IsRowVersion();
        }
    }
}