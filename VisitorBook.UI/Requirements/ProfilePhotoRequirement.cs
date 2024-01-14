using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using VisitorBook.Core.Constants;

namespace VisitorBook.UI.Requirements
{
    public class ProfilePhotoRequirement : IAuthorizationRequirement
    {
    }

    public class ProfilePhotoRequirementHandler : AuthorizationHandler<ProfilePhotoRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProfilePhotoRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == CustomClaims.ProfilePhoto))
            {
                context.Fail();

                return Task.CompletedTask;
            }

            var profilePhotoClaim = context.User.FindFirst(CustomClaims.ProfilePhoto);

            var requestedProfilePhoto = ((AuthorizationFilterContext)context.Resource).HttpContext.Request.Path.Value.Split('/').Last();

            if (profilePhotoClaim.Value != requestedProfilePhoto)
            {
                context.Fail();

                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
