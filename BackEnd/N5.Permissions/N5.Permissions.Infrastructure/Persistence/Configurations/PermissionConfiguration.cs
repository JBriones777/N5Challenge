using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Infrastructure.Persistence.Configurations;

internal class PermissionConfiguration : IEntityTypeConfiguration<Permiso>
{
    public void Configure(EntityTypeBuilder<Permiso> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Permissi__3214EC0792C946FB");

        builder.Property(e => e.ApellidoEmpleado)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(false);
        builder.Property(e => e.FechaPermiso);
        builder.Property(e => e.NombreEmpleado)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(false);

        builder.HasOne(d => d.TipoPermiso).WithMany(p => p.Permissions)
            .HasForeignKey(d => d.TipoPermisoId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Permissions_PermissionTypes");
    }
}
