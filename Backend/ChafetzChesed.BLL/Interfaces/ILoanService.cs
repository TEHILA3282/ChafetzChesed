using ChafetzChesed.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChafetzChesed.BLL.Interfaces
{
    public interface ILoanService
    {
        Task<List<Loan>> GetAllAsync();
        Task<Loan> GetByIdAsync(int id);
        Task<Loan> AddAsync(Loan loan);
        Task<Loan> UpdateAsync(Loan loan);
        Task<bool> DeleteAsync(int id);
    }
}
