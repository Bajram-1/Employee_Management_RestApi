using Employee_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.IServices
{
    public interface IUserService
    {
        Task<DAL.Entities.User> ValidateUser(string userName, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<Employee_Management.BLL.DTO.User>> GetAllUsersAsync();
        Task AddUserAsync(Employee_Management.BLL.DTO.User dtoUser);
        Task UpdateUserAsync(Employee_Management.BLL.DTO.User dtoUser);
        Task DeleteUserAsync(int id);
    }
}
