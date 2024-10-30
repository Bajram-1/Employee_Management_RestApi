using Employee_Management.DAL.Entities;
using Entities;

namespace Employee_Management.DAL.IRepositories
{
    public interface ITaskRepository
    {
        Task<Tasks> GetByIdAsync(int id);
        Task<IEnumerable<Tasks>> GetAllAsync();
        Task AddAsync(Tasks task);
        Task UpdateAsync(Tasks task);
        Task DeleteAsync(Tasks task);
    }
}