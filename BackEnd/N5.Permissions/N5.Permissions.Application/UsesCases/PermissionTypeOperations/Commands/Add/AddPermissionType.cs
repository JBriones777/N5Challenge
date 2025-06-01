using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.UsesCases.PermissionTypeOperations.Commands.Add;

public class AddPermissionType(IUnitOfWork unitOfWork,
    [FromKeyedServices("Add")] IValidator<PermissionTypeDto> validator
    ) : IAddPermissionType
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<PermissionTypeDto> _validator = validator;

    public async Task<PermissionTypeDto> DoIt(PermissionTypeDto permissionType)
    {
        var isValid = await _validator.ValidateAsync(permissionType);
        if (!isValid.IsValid)
            throw new ValidationException(isValid.Errors);
        var entity = permissionType.ToEntity();
        await _unitOfWork.GetRepository<TipoPermiso>().AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.ToDto();
    }
}
