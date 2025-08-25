using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using ChafetzChesed.Common.DTOs;
using System.Text.Json;

namespace ChafetzChesed.BLL.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _email;

        public RegistrationService(AppDbContext context, EmailService email)
        {
            _context = context;
            _email = email;
        }

        public async Task<IEnumerable<Registration>> GetAllAsync()
            => await _context.Registrations.ToListAsync();

        public async Task<Registration?> GetByIdAsync(string id)
            => await _context.Registrations.FirstOrDefaultAsync(r => r.ID == id);

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

        public async Task<bool> UpdateStatusAsync(string registrationId, string newStatus)
        {
            var registration = await _context.Registrations.FindAsync(registrationId);
            if (registration == null) return false;
            registration.RegistrationStatus = newStatus;
            _context.Registrations.Update(registration);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Registration>> GetPendingAsync(int institutionId)
            => await _context.Registrations
                .Where(r => r.RegistrationStatus == "ממתין" && r.InstitutionId == institutionId)
                .ToListAsync();

        public async Task<List<Registration>> GetByStatusAsync(int institutionId, string status)
            => await _context.Registrations
                .Where(r => r.InstitutionId == institutionId && r.RegistrationStatus == status)
                .ToListAsync();

        public async Task<bool> ExistsAsync(string email, string id, int institutionId)
        {
            var normalizedId = id.PadLeft(9, '0');
            return await _context.Registrations.AnyAsync(r =>
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
            var existing = await _context.Registrations.FirstOrDefaultAsync(r => r.ID == userId);
            if (existing == null) return false;

            var changes = new List<object>();

            void Set<T>(string field, T? newVal, T? curVal, Action apply)
            {
                if (newVal is null) return;
                if (!EqualityComparer<T?>.Default.Equals(newVal, curVal))
                {
                    changes.Add(new { field, old = curVal, @new = newVal });
                    apply();
                }
            }

            Set("FirstName", dto.FirstName, existing.FirstName, () => existing.FirstName = dto.FirstName!);
            Set("LastName", dto.LastName, existing.LastName, () => existing.LastName = dto.LastName!);
            Set("PhoneNumber", dto.PhoneNumber, existing.PhoneNumber, () => existing.PhoneNumber = dto.PhoneNumber);
            Set("LandlineNumber", dto.LandlineNumber, existing.LandlineNumber, () => existing.LandlineNumber = dto.LandlineNumber);
            Set("Email", dto.Email, existing.Email, () => existing.Email = dto.Email!);
            Set("DateOfBirth", dto.DateOfBirth, existing.DateOfBirth, () => existing.DateOfBirth = dto.DateOfBirth);
            Set("PersonalStatus", dto.PersonalStatus, existing.PersonalStatus, () => existing.PersonalStatus = dto.PersonalStatus);
            Set("Street", dto.Street, existing.Street, () => existing.Street = dto.Street);
            Set("City", dto.City, existing.City, () => existing.City = dto.City);
            Set("HouseNumber", dto.HouseNumber, existing.HouseNumber, () => existing.HouseNumber = dto.HouseNumber);

            if (changes.Count == 0) return true;

            existing.StatusUpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            _context.AuditLogs.Add(new AuditLog
            {
                InstitutionId = existing.InstitutionId,
                Entity = "Registration",
                EntityId = existing.ID,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = null,
                ChangesJson = JsonSerializer.Serialize(changes)
            });
            await _context.SaveChangesAsync();

            string? managerEmail = null;   

            try
            {
                managerEmail = await _context.Registrations
                    .Where(r => r.InstitutionId == existing.InstitutionId
                             && r.Role == "Admin"
                             && r.RegistrationStatus == "מאושר"
                             && r.Email != null && r.Email != "")
                    .OrderBy(r => r.ID)
                    .Select(r => r.Email!)
                    .FirstOrDefaultAsync();
            }
            catch
            {
            }

            managerEmail ??= "TE32820@GMAIL.COM";

            var subject = $"עדכון פרטי לקוח ({existing.ID}) במוסד {existing.InstitutionId}";
            var body = $"בוצעו שינויים אצל {existing.FirstName} {existing.LastName} ({existing.ID}):\n{JsonSerializer.Serialize(changes)}";

            await _email.SendAsync(managerEmail, subject, body);
            await _email.SendAsync(existing.Email, "העדכון נקלט", "תודה, העדכון נקלט במערכת.");

            return true;
        }

        private async Task SendEmailSafeAsync(string to, string subject, string body)
        {
            try
            {
                await ((dynamic)_email).SendEmailAsync(to, subject, body);
            }
            catch
            {
                try { await ((dynamic)_email).SendAsync(to, subject, body); }
                catch { }
            }
        }
    }
}
