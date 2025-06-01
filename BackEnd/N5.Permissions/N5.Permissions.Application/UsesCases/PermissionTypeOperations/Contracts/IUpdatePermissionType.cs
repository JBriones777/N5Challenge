using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;

namespace N5.Permissions.Application.UsesCases.PermissionTypeOperations.Contracts
{
    public interface IUpdatePermissionType
    {
        Task<PermissionTypeDto> DoIt(PermissionTypeDto permissionType);
    }
}