namespace IdentityServerHost.Pages;

public sealed class SecurityHeadersAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var result = context.Result;
        if (result is PageResult)
        {
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
                context.HttpContext.Response.Headers.Append("X-Content-Type-Options", "nosniff");

            if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
                context.HttpContext.Response.Headers.Append("X-Frame-Options", "DENY");

            var csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";

            if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
                context.HttpContext.Response.Headers.Append("Content-Security-Policy", csp);
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
                context.HttpContext.Response.Headers.Append("X-Content-Security-Policy", csp);

            var referrer_policy = "no-referrer";
            if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
                context.HttpContext.Response.Headers.Append("Referrer-Policy", referrer_policy);
        }
    }
}