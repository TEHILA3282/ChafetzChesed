using ChafetzChesed.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChafetzChesed.BLL.Interfaces
{
    public interface ILoanTypeService
    {
        Task<List<LoanType>> GetAllAsync();
        Task<LoanType> GetByIdAsync(int id);
        Task<LoanType> AddAsync(LoanType loanType);
        Task<LoanType> UpdateAsync(LoanType loanType);
        Task<bool> DeleteAsync(int id);
    }
}
