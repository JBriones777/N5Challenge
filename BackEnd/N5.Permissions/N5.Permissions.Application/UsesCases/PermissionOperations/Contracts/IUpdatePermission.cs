using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;

namespace N5.Permissions.Application.UsesCases.PermissionOperations.Contracts
{
    public interface IUpdatePermission
    {
        Task<PermissionDto> DoIt(PermissionDto permission);
    }
}