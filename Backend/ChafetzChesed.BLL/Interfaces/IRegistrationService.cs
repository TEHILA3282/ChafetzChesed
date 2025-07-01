using ChafetzChesed.DAL.Entities;

namespace ChafetzChesed.BLL.Interfaces
{
    public interface IRegistrationService
    {
        Task<IEnumerable<Registration>> GetAllAsync();
        Task<Registration?> GetByIdAsync(string id);          // במקום int
        Task<Registration> AddAsync(Registration registration);
        Task<bool> UpdateAsync(Registration registration);
        Task<bool> DeleteAsync(string id);                    // במקום int
    }
}
