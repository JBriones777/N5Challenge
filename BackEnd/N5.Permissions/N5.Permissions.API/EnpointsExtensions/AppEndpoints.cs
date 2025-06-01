using Microsoft.AspNetCore.Mvc;
using N5.Permissions.API.Filters;
using N5.Permissions.Application.Dtos;
using N5.Permissions.Application.UsesCases.PermissionOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;

namespace N5.Permissions.API.EnpointsExtensions;

public static class AppEndpoints
{
    public static void MapPermission(this IEndpointRouteBuilder endpoints)
    {
        string basePath = "api/permission";
        endpoints.MapPost(basePath, async (PermissionDto dto, [FromServices] IAddPermission addPermission) =>
        {
            var result = await addPermission.DoIt(dto);
            return Results.Ok(result);
        }).AddEndpointFilter<OperationsFilter>().Produces<PermissionDto>()
        .Produces<ErrorResponseDto>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponseDto>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponseDto>(StatusCodes.Status500InternalServerError);

        endpoints.MapPut(basePath, async (PermissionDto dto, [FromServices] IUpdatePermission updatePermission) =>
        {
            var result = await updatePermission.DoIt(dto);
            return Results.Ok(result);
        }).AddEndpointFilter<OperationsFilter>().Produces<PermissionDto>()
        .Produces<ErrorResponseDto>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponseDto>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponseDto>(StatusCodes.Status500InternalServerError);

        endpoints.MapGet(basePath, async ([FromServices] IGetListPermissions getListPermissions) =>
        {
            var result = await getListPermissions.DoIt();
            return Results.Ok(result);
        }).AddEndpointFilter<OperationsFilter>().Produces<List<GetListPermissionDto>>()
        .Produces<ErrorResponseDto>(StatusCodes.Status500InternalServerError);

        endpoints.MapGet(basePath + "/{id}", async ([FromRoute] int id, [FromServices] IGetByIdPermission getByIdPermission) =>
        {
            var result = await getByIdPermission.DoIt(id);
            return Results.Ok(result);
        }).AddEndpointFilter<OperationsFilter>()
        .Produces<GetPermissionByIdDto>()
        .Produces<ErrorResponseDto>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponseDto>(StatusCodes.Status500InternalServerError);
    }

    public static void MapPermissionType(this IEndpointRouteBuilder endpoints)
    {
        string basePath = "api/permissionType";
        endpoints.MapPost(basePath, async (PermissionTypeDto dto, [FromServices] IAddPermissionType addPermissionType) =>
        {
            var result = await addPermissionType.DoIt(dto);
            return Results.Ok(result);
        }).AddEndpointFilter<OperationsFilter>().Produces<PermissionTypeDto>()
        .Produces<ErrorResponseDto>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponseDto>(StatusCodes.Status500InternalServerError);

        endpoints.MapPut(basePath, async (PermissionTypeDto dto, [FromServices] IUpdatePermissionType updatePermissionType) =>
        {
            var result = await updatePermissionType.DoIt(dto);
            return Results.Ok(result);
        }).AddEndpointFilter<OperationsFilter>().Produces<ErrorResponseDto>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponseDto>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponseDto>(StatusCodes.Status500InternalServerError);

        endpoints.MapGet(basePath, async ([FromServices] IGetListPermissionTypes getListPermissionTypes) =>
        {
            var result = await getListPermissionTypes.DoIt();
            return Results.Ok(result);
        }).AddEndpointFilter<OperationsFilter>().Produces<List<PermissionTypeDto>>()
        .Produces<ErrorResponseDto>(StatusCodes.Status500InternalServerError);

        endpoints.MapGet(basePath + "/{id}", async ([FromRoute] int id, [FromServices] IGetByIdPermissionType getByIdPermissionType) =>
        {
            var result = await getByIdPermissionType.DoIt(id);
            return Results.Ok(result);
        }).AddEndpointFilter<OperationsFilter>().Produces<PermissionTypeDto>()
        .Produces<ErrorResponseDto>(StatusCodes.Status404NotFound).Produces<ErrorResponseDto>(StatusCodes.Status500InternalServerError);
    }
}
