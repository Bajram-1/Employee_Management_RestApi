using Employee_Management.DAL.Entities;
using System.Linq.Expressions;

namespace Employee_Management.DAL.IRepositories
{
    public interface IUserRepository
    {
        Task<User> FindAsync(Expression<Func<User, bool>> predicate);
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string username);
    }
}