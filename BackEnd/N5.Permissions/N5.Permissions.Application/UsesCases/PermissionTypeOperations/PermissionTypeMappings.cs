using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.UsesCases.PermissionTypeOperations;

public static class PermissionTypeMappings
{
    public static TipoPermiso ToEntity(this PermissionTypeDto permissionTypeDto)
    {
        return new TipoPermiso
        {
            Id = permissionTypeDto.Id,
            Descripcion = permissionTypeDto.Descripcion
        };
    }
    
    public static List<PermissionTypeDto> ToDto(this IEnumerable<TipoPermiso> permissionType)
    {
        return [.. permissionType.Select(pt => pt.ToDto())];
    }

    public static PermissionTypeDto ToDto(this TipoPermiso permissionTypeDto)
    {
        return new PermissionTypeDto
        {
            Id = permissionTypeDto.Id,
            Descripcion = permissionTypeDto.Descripcion
        };
    }
}
