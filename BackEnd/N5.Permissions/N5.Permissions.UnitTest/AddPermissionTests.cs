using FluentValidation;
using FluentValidation.Results;
using Moq;
using N5.Permissions.Application.UsesCases.PermissionOperations.Commands.Add;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.UnitTest;

public class AddPermissionTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBaseRepository<TipoPermiso>> _tipoPermisoRepoMock = new();
    private readonly Mock<IBaseRepository<Permiso>> _permisoRepoMock = new();
    private readonly Mock<IValidator<PermissionDto>> _validatorMock = new();

    private AddPermission CreateSut()
    {
        _unitOfWorkMock.Setup(u => u.GetRepository<TipoPermiso>()).Returns(_tipoPermisoRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.GetRepository<Permiso>()).Returns(_permisoRepoMock.Object);
        return new AddPermission(_unitOfWorkMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Should_Throw_ValidationException_When_Model_Is_Invalid()
    {
        // Arrange
        var dto = new PermissionDto();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("NombreEmpleado", "Required") });
        _validatorMock.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        var sut = CreateSut();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => sut.DoIt(dto));
    }

    [Fact]
    public async Task Should_Throw_KeyNotFoundException_When_TipoPermiso_Not_Exists()
    {
        // Arrange
        var dto = new PermissionDto { TipoPermisoId = 99 };
        _validatorMock.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _tipoPermisoRepoMock.Setup(r => r.ExistsAsync(dto.TipoPermisoId)).ReturnsAsync(false);

        var sut = CreateSut();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.DoIt(dto));
    }

    [Fact]
    public async Task Should_Add_Permission_And_Return_Dto_When_Valid()
    {
        // Arrange
        var dto = new PermissionDto { Id = 0, NombreEmpleado = "John", ApellidoEmpleado = "Doe", TipoPermisoId = 1 };
        _validatorMock.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _tipoPermisoRepoMock.Setup(r => r.ExistsAsync(dto.TipoPermisoId)).ReturnsAsync(true);
        var permisoEntity = new Permiso { Id = 1, NombreEmpleado = "John", ApellidoEmpleado = "Doe", TipoPermisoId = 1 };
        _permisoRepoMock.Setup(r => r.AddAsync(It.IsAny<Permiso>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
        var sut = CreateSut();

        // Act
        var result = await sut.DoIt(dto);

        // Assert
        _permisoRepoMock.Verify(r => r.AddAsync(It.IsAny<Permiso>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        Assert.Equal(dto.NombreEmpleado, result.NombreEmpleado);
        Assert.Equal(dto.ApellidoEmpleado, result.ApellidoEmpleado);
        Assert.Equal(dto.TipoPermisoId, result.TipoPermisoId);
    }
}
