namespace ApiGateway.Middleware;

/// <summary>
/// Forwards the incoming Authorization header to downstream services.
/// </summary>
public class JwtForwardMiddleware
{
    private readonly RequestDelegate _next;
    public JwtForwardMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext ctx)
    {
        if (ctx.Request.Headers.TryGetValue("Authorization", out var auth))
            ctx.Items["ForwardedAuth"] = auth.ToString();
        await _next(ctx);
    }
}
