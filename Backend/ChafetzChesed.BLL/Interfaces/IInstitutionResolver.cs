using System.Threading;
using System.Threading.Tasks;
using ChafetzChesed.DAL.Entities;

namespace ChafetzChesed.BLL.Services
{
    public interface IInstitutionResolver
    {  
        Task<Institution?> GetBySubdomainAsync(string subdomain, CancellationToken ct = default);
    }
}

