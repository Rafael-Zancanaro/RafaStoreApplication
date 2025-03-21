﻿using System.Net;
using Polly.CircuitBreaker;
using Refit;

namespace RafaStore.WebApp.MVC.Extensions;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (CustomHttpRequestException ex)
        {
            HandleRequestExceptionAsync(httpContext, ex.StatusCode);
        }
        catch (ValidationApiException ex)
        {
            HandleRequestExceptionAsync(httpContext, ex.StatusCode);
        }
        catch (ApiException ex)
        {
            HandleRequestExceptionAsync(httpContext, ex.StatusCode);
        }
        catch (BrokenCircuitException)
        {
            HandleCircuitBrakerExceptionAsync(httpContext);
        }
    }

    private static void HandleRequestExceptionAsync(HttpContext httpContext, HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.Unauthorized)
        {
            httpContext.Response.Redirect($"/login?ReturnUrl={httpContext.Request.Path}");
            return;
        }

        httpContext.Response.StatusCode = (int)statusCode;
    }
    
    private static void HandleCircuitBrakerExceptionAsync(HttpContext httpContext)
    {
        httpContext.Response.Redirect("/sistema-indisponivel");
    }
}