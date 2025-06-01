using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using N5.Permissions.Application.UsesCases.PermissionOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.UsesCases.PermissionOperations.Commands.Add;

public class AddPermission(IUnitOfWork unitOfWork,
    [FromKeyedServices("Add")] IValidator<PermissionDto> validator
    ) : IAddPermission
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<PermissionDto> _validator = validator;

    public async Task<PermissionDto> DoIt(PermissionDto permission)
    {
        var isValid = await _validator.ValidateAsync(permission);
        if (!isValid.IsValid)
            throw new ValidationException(isValid.Errors);

        var existEntity = await _unitOfWork.GetRepository<TipoPermiso>().ExistsAsync(permission.TipoPermisoId);
        if (!existEntity)
            throw new KeyNotFoundException($"Tipo de permiso: {permission.Id} no existe.");

        var entity = permission.ToEntity();
        await _unitOfWork.GetRepository<Permiso>().AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.ToDto();
    }
}
