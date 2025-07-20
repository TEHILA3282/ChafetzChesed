using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChafetzChesed.BLL.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly AppDbContext _context;

        public RegistrationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Registration>> GetAllAsync()
        {
            return await _context.Registrations.ToListAsync();
        }

        public async Task<Registration?> GetByIdAsync(string id)
        {
            return await _context.Registrations.FindAsync(id);
        }

        public async Task<Registration> AddAsync(Registration registration)
        {
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task<bool> UpdateAsync(Registration registration)
        {
            _context.Registrations.Update(registration);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var reg = await _context.Registrations.FindAsync(id);
            if (reg == null) return false;

            _context.Registrations.Remove(reg);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Registration>> GetPendingAsync(int institutionId)
        {
            return await _context.Registrations
                .Where(r => r.RegistrationStatus == "ממתין" && r.InstitutionId == institutionId)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(string registrationId, string newStatus)
        {
            var user = await _context.Registrations.FindAsync(registrationId);
            if (user == null) return false;

            user.RegistrationStatus = newStatus;
            _context.Registrations.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> ExistsAsync(string email, string id, int institutionId)
        {
            var all = await GetAllAsync(); 
            return all.Any(r =>
                r.InstitutionId == institutionId &&
                (r.Email == email || r.ID == id) &&
                r.RegistrationStatus != "נדחה"
            );
        }
    }
}
