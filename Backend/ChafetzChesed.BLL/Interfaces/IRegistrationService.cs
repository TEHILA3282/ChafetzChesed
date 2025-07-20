using ChafetzChesed.DAL.Entities;

namespace ChafetzChesed.BLL.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<Registration>> GetAllAsync();
        Task<Registration?> GetByIdAsync(string id);        
        Task<Registration> AddAsync(Registration registration);
        Task<bool> UpdateAsync(Registration registration);
        Task<bool> DeleteAsync(string id);
        Task<List<Registration>> GetPendingAsync(int institutionId);
        Task<bool> UpdateStatusAsync(string registrationId, string newStatus);
        Task<bool> ExistsAsync(string email, string id, int institutionId);

    }
}
