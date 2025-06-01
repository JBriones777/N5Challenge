using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.UsesCases.PermissionOperations;

public static class PermissionMappings
{
    public static Permiso ToEntity(this PermissionDto permissionTypeDto)
    {
        return new Permiso
        {
            Id = permissionTypeDto.Id,
            NombreEmpleado = permissionTypeDto.NombreEmpleado,
            ApellidoEmpleado = permissionTypeDto.ApellidoEmpleado,
            TipoPermisoId = permissionTypeDto.TipoPermisoId
        };
    }


    public static PermissionDto ToDto(this Permiso permission)
    {
        return new PermissionDto
        {
            Id = permission.Id,
            NombreEmpleado = permission.NombreEmpleado,
            ApellidoEmpleado = permission.ApellidoEmpleado,
            TipoPermisoId = permission.TipoPermisoId
        };
    }

    public static GetPermissionByIdDto ToSingleDto(this Permiso permission)
    {
        return new GetPermissionByIdDto
        {
            Id = permission.Id,
            NombreEmpleado = permission.NombreEmpleado,
            ApellidoEmpleado = permission.ApellidoEmpleado,
            TipoPermisoId = permission.TipoPermisoId,
            FechaPermiso = permission.FechaPermiso
        };
    }
}
