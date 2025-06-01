using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;
using N5.Permissions.IntegrationTest.Supports;
using System.Net.Http.Json;

namespace N5.Permissions.IntegrationTest.Permission;

public class RegisterPermission : Given_When_Then_Test_Async, IClassFixture<IntegrationSupport>
{
    private readonly HttpClient _client;
    private HttpResponseMessage _result;
    private PermissionDto _permission;
    public RegisterPermission(IntegrationSupport fixture)
    {
        _client = fixture.Client;
        InitializeAsync().GetAwaiter().GetResult();
    }

    protected override async Task Given()
    {
        PermissionTypeDto permissionType = new()
        {
            Descripcion = "Vacaciones"
        };
        var result = await _client.PostAsJsonAsync("/api/PermissionType", permissionType);
        result.EnsureSuccessStatusCode();
        var permissionTypeDto = await result.Content.ReadFromJsonAsync<PermissionTypeDto>();
        _permission = new PermissionDto
        {
            NombreEmpleado = "John",
            ApellidoEmpleado = "Doe",
            TipoPermisoId = permissionTypeDto.Id,
        };
    }

    protected override async Task When()
    {
        _result = await _client.PostAsJsonAsync("/api/Permission", _permission);
        _result.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task Then_Should_Return_Created_Permission()
    {
        Assert.Equal(System.Net.HttpStatusCode.OK, _result.StatusCode);
        var result = await _result.Content.ReadFromJsonAsync<PermissionDto>();
        Assert.NotNull(result);
        Assert.Equal(_permission.NombreEmpleado, result.NombreEmpleado);
        Assert.Equal(_permission.ApellidoEmpleado, result.ApellidoEmpleado);
        Assert.Equal(_permission.TipoPermisoId, result.TipoPermisoId);
        
    }

}
