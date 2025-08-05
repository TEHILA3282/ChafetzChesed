using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using ChafetzChesed.Common.DTOs;
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
            return await _context.Registrations.FirstOrDefaultAsync(r => r.ID == id);
        }

        public async Task<Registration> AddAsync(Registration registration)
        {
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var reg = await _context.Registrations.FindAsync(id);
            if (reg == null) return false;

            _context.Registrations.Remove(reg);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateStatusAsync(string RegistrationId, string newStatus)
        {
            var registration = await _context.Registrations.FindAsync(RegistrationId);
            if (registration == null)
                return false;

            registration.RegistrationStatus = newStatus;
            _context.Registrations.Update(registration);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<List<Registration>> GetPendingAsync(int institutionId)
        {
            return await _context.Registrations
                .Where(r => r.RegistrationStatus == "ממתין" && r.InstitutionId == institutionId)
                .ToListAsync();
        }

        public async Task<List<Registration>> GetByStatusAsync(int institutionId, string status)
        {
            return await _context.Registrations
                .Where(r => r.InstitutionId == institutionId && r.RegistrationStatus == status)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string email, string id, int institutionId)
        {
            var normalizedId = id.PadLeft(9, '0');
            var all = await GetAllAsync();
            return all.Any(r =>
                r.InstitutionId == institutionId &&
                (r.Email == email || r.ID == normalizedId) &&
                r.RegistrationStatus != "נדחה"
            );
        }
        public async Task<bool> UpdateAsync(Registration updated)
        {
            var existing = await _context.Registrations.FindAsync(updated.ID);
            if (existing == null) return false;

            updated.Password = existing.Password;

            _context.Entry(existing).CurrentValues.SetValues(updated);

            existing.StatusUpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdatePartialAsync(string userId, RegistrationUpdateDto dto)
        {
            var existing = await _context.Registrations.FindAsync(userId);
            if (existing == null) return false;

            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.PhoneNumber = dto.PhoneNumber;
            existing.LandlineNumber = dto.LandlineNumber;
            existing.Email = dto.Email;
            existing.DateOfBirth = dto.DateOfBirth;
            existing.PersonalStatus = dto.PersonalStatus;
            existing.Street = dto.Street;
            existing.City = dto.City;
            existing.HouseNumber = dto.HouseNumber;
            existing.StatusUpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }



    }
}
