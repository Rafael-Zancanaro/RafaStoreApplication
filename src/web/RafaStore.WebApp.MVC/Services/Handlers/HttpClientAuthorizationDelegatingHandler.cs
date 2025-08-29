using System.Net.Http.Headers;
using RafaStore.WebAPI.Core.Usuario;
using RafaStore.WebApp.MVC.Extensions;

namespace RafaStore.WebApp.MVC.Services.Handlers;

public class HttpClientAuthorizationDelegatingHandler(IAspNetUser user) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authorization = user.ObterHttpContext().Request.Headers.Authorization;

        if (!string.IsNullOrWhiteSpace(authorization))
            request.Headers.Add("Authorization", new List<string> { authorization });

        var token = user.ObterUserToken();

        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        return base.SendAsync(request, cancellationToken);
    }
}