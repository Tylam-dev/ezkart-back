using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistencia.Entidades;

public class RefreshToken : EntidadBase
{
    [Required]
    [Key]
    public int Id { get; set; }
    [Required]
    [Column("token_hash")]
    public string TokenHash { get; set; } = null!;
    [Required]
    [Column("expiracion")]
    public DateTime Expiracion { get; set; }
    [Required]
    [Column("usuario_id")]
    public Guid UsuarioId {get;set;}
    public virtual Usuario Usuario { get; set;}
    public class UsuarioConfiguracion : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasOne(x => x.Usuario)
                .WithMany(x => x.RefreshToken)
                .HasForeignKey(x => x.UsuarioId);
        }
    }
}