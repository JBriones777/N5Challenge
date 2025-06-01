using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;

namespace N5.Permissions.Application.UsesCases.PermissionTypeOperations.Contracts
{
    public interface IGetByIdPermissionType
    {
        Task<PermissionTypeDto> DoIt(int id);
    }
}