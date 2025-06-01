using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Infrastructure.Persistence.Configurations;

internal class PermissionTypeConfiguration : IEntityTypeConfiguration<TipoPermiso>
{
    public void Configure(EntityTypeBuilder<TipoPermiso> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__Permissi__3214EC07B6540850");

        builder.Property(e => e.Descripcion)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(false);
    }
}
