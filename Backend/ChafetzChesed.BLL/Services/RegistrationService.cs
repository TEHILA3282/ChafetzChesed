using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
            ValidateRegistration(registration);

            if (await _context.Registrations.AnyAsync(r => r.ID == registration.ID))
                throw new Exception("User with this ID already exists.");

            registration.Password = HashPassword(registration.Password);
            registration.StatusUpdatedAt = DateTime.Now;

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task<bool> UpdateAsync(Registration registration)
        {
            var existing = await _context.Registrations.FindAsync(registration.ID);
            if (existing == null)
                return false;

            existing.FirstName = registration.FirstName;
            existing.LastName = registration.LastName;
            existing.PhoneNumber = registration.PhoneNumber;
            existing.LandlineNumber = registration.LandlineNumber;
            existing.Email = registration.Email;
            existing.DateOfBirth = registration.DateOfBirth;
            existing.PersonalStatus = registration.PersonalStatus;
            existing.Street = registration.Street;
            existing.City = registration.City;
            existing.HouseNumber = registration.HouseNumber;
            existing.RegistrationStatus = registration.RegistrationStatus;
            existing.StatusUpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
                return false;

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            return true;
        }

        private void ValidateRegistration(Registration r)
        {
            if (string.IsNullOrWhiteSpace(r.ID) || r.ID.Length != 9)
                throw new Exception("Invalid ID – must be 9 digits.");
            if (string.IsNullOrWhiteSpace(r.FirstName))
                throw new Exception("First name is required.");
            if (string.IsNullOrWhiteSpace(r.LastName))
                throw new Exception("Last name is required.");
            if (string.IsNullOrWhiteSpace(r.Password) || r.Password.Length < 6)
                throw new Exception("Password is required and must be at least 6 characters.");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
