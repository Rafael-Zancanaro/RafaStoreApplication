using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace RafaStore.WebAPI.Core.Identidade;

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
    {
        Arguments = [new Claim(claimName, claimValue)];
    }
}

public class RequisitoClaimFilter(Claim claim) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new StatusCodeResult(401);
            return;
        }

        if (!CustomAutorize.ValidarClaimsUsuario(context.HttpContext, claim.Type, claim.Value))
            context.Result = new StatusCodeResult(403);
    }
}
public class CustomAutorize
{
    public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        => context.User.Identity.IsAuthenticated &&
           context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
}
