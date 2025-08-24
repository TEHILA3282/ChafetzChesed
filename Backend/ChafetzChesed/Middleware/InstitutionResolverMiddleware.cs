using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using ChafetzChesed.DAL.Data;

public class InstitutionResolverMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly ILogger<InstitutionResolverMiddleware> _logger;

    private const int DefaultInstitutionId = 1;
    private static readonly Regex SlugRegex =
        new(@"^[a-z0-9-]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public InstitutionResolverMiddleware(
        RequestDelegate next,
        IMemoryCache cache,
        ILogger<InstitutionResolverMiddleware> logger)
    {
        _next = next;
        _cache = cache;
        _logger = logger;
    }

  public async Task Invoke(HttpContext context, AppDbContext db)
    {
        try
        {
            int? instId = await ResolveInstitutionIdAsync(context, db);
            if (instId is int ok && ok > 0)
            {
                context.Items["InstitutionId"] = ok;
            }
            else
            {
                _logger.LogWarning("InstitutionId could not be resolved for {Path} | Host={Host}",
                    context.Request.Path, context.Request.Host);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed resolving InstitutionId");
        }

        await _next(context);
    }

    private async Task<int?> ResolveInstitutionIdAsync(HttpContext ctx, AppDbContext db)
    {
        if (ctx.Request.Headers.TryGetValue("X-Institution-Id", out StringValues idHeader) &&
            int.TryParse(idHeader.ToString(), out var fromHeaderId) && fromHeaderId > 0)
            return fromHeaderId;

        if (ctx.Request.Headers.TryGetValue("X-Institution-Slug", out var slugHeader))
        {
            var slug = slugHeader.ToString().Trim().ToLowerInvariant();
            var found = await FindBySlugAsync(db, slug);
            if (found is int okId) return okId;
        }

        if (ctx.Request.Headers.TryGetValue("Referer", out var referer) &&
            Uri.TryCreate(referer.ToString(), UriKind.Absolute, out var refUri))
        {
            var fromRef = await ResolveFromPathAsync(db, refUri.AbsolutePath);
            if (fromRef is int okRef) return okRef;
        }

        var fromPath = await ResolveFromPathAsync(db, ctx.Request.Path.Value ?? "");
        if (fromPath is int okPath) return okPath;

        var host = ctx.Request.Host.Host?.ToLowerInvariant() ?? "";
        if (host.Contains("c-chesed.org.il")) return DefaultInstitutionId;
        if (host is "localhost" or "127.0.0.1") return DefaultInstitutionId;

        return null;
    }

    private async Task<int?> ResolveFromPathAsync(AppDbContext db, string path)
    {
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0) return null;
        if (segments.Length >= 3 &&
    string.Equals(segments[0], "api", StringComparison.OrdinalIgnoreCase) &&
    int.TryParse(segments[2], out var apiId) && apiId > 0)
        {
            return apiId;
        }

        var first = segments[0].ToLowerInvariant();
        var candidate = first == "api" && segments.Length >= 2
            ? segments[1].ToLowerInvariant()
            : first;

        if (string.IsNullOrWhiteSpace(candidate) || !SlugRegex.IsMatch(candidate))
            return null;

        return await FindBySlugAsync(db, candidate);
    }

    private Task<int?> FindBySlugAsync(AppDbContext db, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return Task.FromResult<int?>(null);

        var cacheKey = $"inst:slug:{slug}";
        if (_cache.TryGetValue(cacheKey, out int cachedId))
            return Task.FromResult<int?>(cachedId);

        return LoadAndCacheAsync(db, slug, cacheKey);
    }

    private async Task<int?> LoadAndCacheAsync(AppDbContext db, string slug, string cacheKey)
    {
        var entity = await db.Institutions
            .AsNoTracking()
            .Where(i => i.IsActive) 
            .FirstOrDefaultAsync(i => i.Subdomain != null && i.Subdomain.ToLower() == slug);

        if (entity == null) return null;

        _cache.Set(cacheKey, entity.InstitutionId, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        return entity.InstitutionId;
    }
}
