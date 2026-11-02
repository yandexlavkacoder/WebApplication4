using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication4.Authorization;

public class OwnerOrAdminHandler : AuthorizationHandler<OwnerOrAdminRequirement, IOwnedResource>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnerOrAdminRequirement requirement,
        IOwnedResource resource)
    {
        if (context.User.IsInRole(Roles.Admin))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is not null && userId == resource.OwnerUserId.ToString())
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
