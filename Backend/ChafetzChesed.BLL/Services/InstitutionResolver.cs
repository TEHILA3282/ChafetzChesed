using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.DAL.Entities;

namespace ChafetzChesed.BLL.Services
{


    public class InstitutionResolver : IInstitutionResolver
    {
        private readonly AppDbContext _db;
        private readonly IMemoryCache _cache;

        public InstitutionResolver(AppDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<Institution?> GetBySubdomainAsync(string subdomain, CancellationToken ct = default)
        {
            subdomain = (subdomain ?? "").Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(subdomain)) subdomain = "localhost";

            var key = $"inst:sub:{subdomain}";
            if (_cache.TryGetValue(key, out Institution inst))
                return inst;

            inst = await _db.Institutions
                            .Where(i => i.IsActive && i.Subdomain == subdomain)
                            .FirstOrDefaultAsync(ct);

            _cache.Set(key, inst, TimeSpan.FromMinutes(5));
            return inst;
        }
    }
}
