using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.UsesCases.PermissionTypeOperations.Queries.GetList;

public class GetListPermissionTypes(IUnitOfWork unitOfWork) : IGetListPermissionTypes
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<PermissionTypeDto>> DoIt()
    {
        var result = await _unitOfWork.GetRepository<TipoPermiso>().GetListAsync(false);

        return result.ToDto();
    }
}
