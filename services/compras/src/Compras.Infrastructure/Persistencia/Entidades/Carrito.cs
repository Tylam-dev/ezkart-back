using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Compras.Infrastructure.Persistencia.Entidades;
[Table("carrito")]
public class Carrito : EntidadBase
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Required]
    [Column("usuario_id")]
    public Guid UsuarioId { get; set; }
    public virtual ICollection<CarritoItem> Items { get; set; } = new List<CarritoItem>();
    public class CarritoConfiguracion : EntidadBaseConfiguration<Carrito>
    {
        public override void Configure(EntityTypeBuilder<Carrito> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.UsuarioId)
                .IsUnique();

            builder.HasMany(x => x.Items)
                .WithOne(x => x.Carrito)
                .HasForeignKey(x => x.CarritoId);
        }
    }
}
