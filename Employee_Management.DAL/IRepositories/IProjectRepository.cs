using Employee_Management.DAL.Entities;

namespace Employee_Management.DAL.IRepositories
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(int id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(int id);
    }
}