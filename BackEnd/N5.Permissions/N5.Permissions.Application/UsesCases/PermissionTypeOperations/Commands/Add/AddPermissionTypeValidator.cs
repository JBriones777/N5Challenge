using FluentValidation;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;

namespace N5.Permissions.Application.UsesCases.PermissionTypeOperations.Commands.Add;

public class AddPermissionTypeValidator : AbstractValidator<PermissionTypeDto>
{
    public AddPermissionTypeValidator()
    {
        RuleFor(x => x.Id)
            .Equal(0).WithMessage("El Id no puede ser distinto de 0.");
        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(255).WithMessage("La descripción no puede exceder los 255 caracteres.");
    }
}