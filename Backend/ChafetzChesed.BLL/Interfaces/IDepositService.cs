using ChafetzChesed.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChafetzChesed.BLL.Interfaces
{
    public interface IDepositService
    {
        Task<List<Deposit>> GetAllAsync();
        Task<Deposit> GetByIdAsync(int id);
        Task<Deposit> AddAsync(Deposit deposit);
        Task<Deposit> UpdateAsync(Deposit deposit);
        Task<bool> DeleteAsync(int id);
    }
}
