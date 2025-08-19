namespace ChafetzChesed.Middleware
{
    using ChafetzChesed.DAL.Entities;
    using ChafetzChesed.BLL.Services;
    public class InstitutionMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly HashSet<string> _bypass = new(StringComparer.OrdinalIgnoreCase)
        {
            "swagger", "health", "favicon.ico"
        };

        public InstitutionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, IInstitutionResolver resolver, ILogger<InstitutionMiddleware> logger)
        {
            var path = context.Request.Path.Value ?? "/";
            var segs = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segs.Length > 0 && _bypass.Contains(segs[0]))
            {
                await _next(context);
                return;
            }

            var host = (context.Request.Host.Host ?? string.Empty).Trim().ToLowerInvariant();

            var sub = ExtractSubdomain(host);

            if (string.IsNullOrEmpty(sub))
            {
               
                logger.LogWarning("No subdomain in host {Host}", host);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Institution not found (no subdomain)");
                return;
            }

           
            var institution = await resolver.GetBySubdomainAsync(sub, context.RequestAborted);

            if (institution == null)
            {
                logger.LogWarning("Institution not found for subdomain {Subdomain}", sub);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync($"Institution '{sub}' not found");
                return;
            }

            context.Items["Institution"] = institution;
            context.Items["InstitutionId"] = institution.ID; 

            await _next(context);
        }

        private static string? ExtractSubdomain(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
                return null;
            if (host == "localhost" || System.Net.IPAddress.TryParse(host, out _))
                return "localhost";

            var parts = host.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3)
                return null; 

            if (parts[0] == "www" && parts.Length >= 4)
                return parts[1];

            return parts[0];
        }
    }
}
