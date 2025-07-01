using ChafetzChesed.DAL.Data;
using ChafetzChesed.DAL.Entities;
using ChafetzChesed.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChafetzChesed.BLL.Services
{
    public class DepositService : IDepositService
    {
        private readonly AppDbContext _context;

        public DepositService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Deposit>> GetAllAsync() =>
            await _context.Deposits.ToListAsync();

        public async Task<Deposit> GetByIdAsync(int id) =>
            await _context.Deposits.FindAsync(id);

        public async Task<Deposit> AddAsync(Deposit deposit)
        {
            _context.Deposits.Add(deposit);
            await _context.SaveChangesAsync();
            return deposit;
        }

        public async Task<Deposit> UpdateAsync(Deposit deposit)
        {
            _context.Deposits.Update(deposit);
            await _context.SaveChangesAsync();
            return deposit;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Deposits.FindAsync(id);
            if (entity == null) return false;

            _context.Deposits.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
