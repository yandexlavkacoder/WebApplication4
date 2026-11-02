using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication4.Swagger;

public class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var method = context.MethodInfo;
        var attributes = method.GetCustomAttributes(true)
            .Concat(method.DeclaringType?.GetCustomAttributes(true) ?? []);

        var requiresAuth = attributes.OfType<AuthorizeAttribute>().Any();
        var allowsAnonymous = attributes.OfType<AllowAnonymousAttribute>().Any();
        if (!requiresAuth || allowsAnonymous) return;

        operation.Security ??= new List<OpenApiSecurityRequirement>();
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = new List<string>(),
        });

        operation.Responses ??= new OpenApiResponses();
        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Нет токена или токен недействителен" });
        operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Недостаточно прав" });
    }
}
