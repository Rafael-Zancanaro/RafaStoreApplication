using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RafaStore.WebAPI.Core.Usuario;

public class AspNetUser(IHttpContextAccessor contextAccessor) : IAspNetUser
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public string Name => _contextAccessor.HttpContext.User.Identity.Name;

    public bool EstaAutenticado()
    {
        return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public IEnumerable<Claim> ObterClaims()
    {
        return _contextAccessor.HttpContext.User.Claims;
    }

    public HttpContext ObterHttpContext()
    {
        return _contextAccessor.HttpContext;
    }

    public string ObterUserEmail()
    {
        return EstaAutenticado() ? _contextAccessor.HttpContext.User.GetUserEmail() : string.Empty;
    }

    public Guid ObterUserId()
    {
        return EstaAutenticado() ? Guid.Parse(_contextAccessor.HttpContext.User.GetUserId()) : Guid.Empty;
    }

    public string ObterUserToken()
    {
        return EstaAutenticado() ? _contextAccessor.HttpContext.User.GetUserToken() : string.Empty;
    }

    public bool PossuiRole(string role)
    {
        return _contextAccessor.HttpContext.User.IsInRole(role);
    }
}