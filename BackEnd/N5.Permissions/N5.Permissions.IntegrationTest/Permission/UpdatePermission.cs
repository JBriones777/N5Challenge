using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;
using N5.Permissions.IntegrationTest.Supports;
using System.Net.Http.Json;

namespace N5.Permissions.IntegrationTest.Permission;

public class UpdatePermission : Given_When_Then_Test_Async, IClassFixture<IntegrationSupport>
{
    private readonly HttpClient _client;
    private readonly IntegrationSupport _fixture;
    private HttpResponseMessage _result;
    private PermissionDto _permissionToUpdate;
    public UpdatePermission(IntegrationSupport fixture)
    {
        _client = fixture.Client;
        InitializeAsync().GetAwaiter().GetResult();
        this._fixture = fixture;
    }

    protected override async Task Given()
    {
        PermissionTypeDto permissionType = new()
        {
            Descripcion = "Vacaciones"
        };
        var resultPermissionType = await _client.PostAsJsonAsync("/api/PermissionType", permissionType);
        resultPermissionType.EnsureSuccessStatusCode();
        var permissionTypeDto = await resultPermissionType.Content.ReadFromJsonAsync<PermissionTypeDto>();
        var permission = new PermissionDto
        {
            NombreEmpleado = "John",
            ApellidoEmpleado = "Doe",
            TipoPermisoId = permissionTypeDto.Id,
        };
        var resultPermission = await _client.PostAsJsonAsync("/api/Permission", permission);
        resultPermission.EnsureSuccessStatusCode();
        _permissionToUpdate = await resultPermission.Content.ReadFromJsonAsync<PermissionDto>();
        _permissionToUpdate.NombreEmpleado = "Jane";
    }

    protected override async Task When()
    {
        _result = await _client.PutAsJsonAsync("/api/Permission", _permissionToUpdate);
        _result.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Then_UpdatePermission_ShouldReturnUpdatedPermission()
    {
        var updatedPermission = await _result.Content.ReadFromJsonAsync<PermissionDto>();
        Assert.NotNull(updatedPermission);
        Assert.Equal(_permissionToUpdate.Id, updatedPermission.Id);
        Assert.Equal(_permissionToUpdate.NombreEmpleado, updatedPermission.NombreEmpleado);
        Assert.Equal(_permissionToUpdate.ApellidoEmpleado, updatedPermission.ApellidoEmpleado);
        Assert.Equal(_permissionToUpdate.TipoPermisoId, updatedPermission.TipoPermisoId);
    }

}
