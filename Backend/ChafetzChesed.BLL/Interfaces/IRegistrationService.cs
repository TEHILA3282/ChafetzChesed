using ChafetzChesed.DAL.Entities;
using ChafetzChesed.Common.DTOs;


namespace ChafetzChesed.BLL.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<Registration>> GetAllAsync();
        Task<Registration?> GetByIdAsync(string id);        
        Task<Registration> AddAsync(Registration registration);
        Task<bool> UpdateAsync(Registration registration);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateStatusAsync(string RegistrationId, string newStatus);
        Task<List<Registration>> GetPendingAsync(int institutionId);
        Task<bool> ExistsAsync(string email, string id, int institutionId);
        Task<List<Registration>> GetByStatusAsync(int institutionId, string status);
        Task<bool> UpdatePartialAsync(string userId, RegistrationUpdateDto dto);


    }
}
