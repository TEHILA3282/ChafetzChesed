using ChafetzChesed.DAL.Data;
using ChafetzChesed.DAL.Entities;
using ChafetzChesed.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChafetzChesed.BLL.Services
{
    public class LoanService : ILoanService
    {
        private readonly AppDbContext _context;

        public LoanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Loan>> GetAllAsync() =>
            await _context.Loans.ToListAsync();

        public async Task<Loan> GetByIdAsync(int id) =>
            await _context.Loans.FindAsync(id);

        public async Task<Loan> AddAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<Loan> UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Loans.FindAsync(id);
            if (entity == null) return false;

            _context.Loans.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
