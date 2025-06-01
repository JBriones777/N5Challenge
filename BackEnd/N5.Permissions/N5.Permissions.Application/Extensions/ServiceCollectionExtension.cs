using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using N5.Permissions.Application.UsesCases.PermissionOperations.Commands.Add;
using N5.Permissions.Application.UsesCases.PermissionOperations.Commands.Update;
using N5.Permissions.Application.UsesCases.PermissionOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Application.UsesCases.PermissionOperations.Queries.GetById;
using N5.Permissions.Application.UsesCases.PermissionOperations.Queries.GetList;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Commands.Add;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Commands.Update;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Queries.GetById;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Queries.GetList;

namespace N5.Permissions.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<IValidator<PermissionTypeDto>, AddPermissionTypeValidator>("Add");
        services.AddKeyedScoped<IValidator<PermissionTypeDto>, UpdatePermissionTypeValidator>("Update");      
        services.AddKeyedScoped<IValidator<PermissionDto>, AddPermissionValidator>("Add");
        services.AddKeyedScoped<IValidator<PermissionDto>, UpdatePermissionValidator>("Update");
        services.AddScoped<IAddPermissionType, AddPermissionType>();
        services.AddScoped<IUpdatePermissionType, UpdatePermissionType>();
        services.AddScoped<IGetListPermissionTypes, GetListPermissionTypes>();
        services.AddScoped<IGetByIdPermissionType, GetByIdPermissionType>();
        services.AddScoped<IGetListPermissions, GetListPermissions>();
        services.AddScoped<IGetByIdPermission, GetByIdPermission>();
        services.AddScoped<IAddPermission, AddPermission>();
        services.AddScoped<IUpdatePermission, UpdatePermission>();
        return services;
    }
}