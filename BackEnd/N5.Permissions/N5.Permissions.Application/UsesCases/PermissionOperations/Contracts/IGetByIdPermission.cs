using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;

namespace N5.Permissions.Application.UsesCases.PermissionOperations.Contracts
{
    public interface IGetByIdPermission
    {
        Task<GetPermissionByIdDto> DoIt(int id);
    }
}