using Employee_Management.DAL.Entities;

namespace Employee_Management.DAL.IRepositories
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetByIdAsync(int userId);
        Task<IEnumerable<UserProfile>> GetAllAsync();
        Task AddAsync(UserProfile userProfile);
        Task UpdateAsync(UserProfile userProfile);
        Task DeleteAsync(int userId);
    }
}