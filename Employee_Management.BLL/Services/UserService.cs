using Employee_Management.BLL.DTO;
using Employee_Management.BLL.IServices;
using Employee_Management.Common.Enums;
using Employee_Management.DAL.Entities;
using Employee_Management.DAL.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<DAL.Entities.User> ValidateUser(string userName, string password)
        {
            return await _userRepository.FindAsync(user => user.Email == userName && user.PasswordHash == password);
        }

        public async Task<DAL.Entities.User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Employee_Management.BLL.DTO.User>> GetAllUsersAsync()
        {
            var entityUsers = await _userRepository.GetAllAsync();
            var dtoUsers = entityUsers.Select(user => new Employee_Management.BLL.DTO.User
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = Enum.TryParse<UserRole>(user.Role, out var role) ? role : UserRole.Employee,
                City = user.City,
                PhoneNumber = user.PhoneNumber
            });

            return dtoUsers;
        }

        public async Task AddUserAsync(DTO.User dtoUser)
        {
            var entityUser = new DAL.Entities.User
            {
                UserName = dtoUser.UserName,
                Email = dtoUser.Email,
                EmailConfirmed = true,
                PasswordHash = dtoUser.PasswordHash,
                Role = dtoUser.Role.ToString(),
                City = dtoUser.City,
                PhoneNumber = dtoUser.PhoneNumber
            };
            await _userRepository.AddAsync(entityUser);
        }

        public async Task UpdateUserAsync(DTO.User dtoUser)
        {
            var entityUser = new DAL.Entities.User
            {
                Id = dtoUser.Id,
                UserName = dtoUser.UserName,
                Email = dtoUser.Email,
                PasswordHash = dtoUser.PasswordHash,
                Role = dtoUser.Role.ToString(),
                City = dtoUser.City,
                PhoneNumber = dtoUser.PhoneNumber,
                ProfilePicture = dtoUser.ProfilePicture
            };

            try
            {
                await _userRepository.UpdateAsync(entityUser);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("Concurrency conflict detected. Please try again.", ex);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}