using FluentValidation;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;

namespace N5.Permissions.Application.UsesCases.PermissionOperations.Commands.Update;

public class UpdatePermissionValidator : AbstractValidator<PermissionDto>
{
    public UpdatePermissionValidator()
    {
        RuleFor(x=>x.Id)
            .GreaterThan(0).WithMessage("El Id tiene que ser distinto de 0.");
        RuleFor(x => x.NombreEmpleado)
            .NotEmpty().WithMessage("El nombre del empleado es obligatorio.")
            .MaximumLength(255).WithMessage("El nombre del empleado no puede exceder los 255 caracteres.");
        RuleFor(x => x.ApellidoEmpleado)
            .NotEmpty().WithMessage("El apellido del empleado es obligatorio.")
            .MaximumLength(255).WithMessage("El apellido del empleado no puede exceder los 255 caracteres.");
        RuleFor(x => x.TipoPermisoId)
            .GreaterThan(0).WithMessage("El tipo de permiso debe ser un valor distinto de 0.");
    }
}
