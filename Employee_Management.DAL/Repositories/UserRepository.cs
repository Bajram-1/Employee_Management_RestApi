using Employee_Management.DAL.Entities;
using Employee_Management.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Employee_Management.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> FindAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.SingleOrDefaultAsync(predicate);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser == null)
            {
                throw new Exception("User not found or has been deleted.");
            }

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role.ToString();
            existingUser.City = user.City;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.ProfilePicture = user.ProfilePicture;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var databaseValues = entry.GetDatabaseValues();

                if (databaseValues == null)
                {
                    throw new Exception("Concurrency conflict detected. The user might have been deleted.");
                }

                var databaseUser = (User)databaseValues.ToObject();
                user.RowVersion = (byte[])databaseUser.RowVersion;

                throw new DbUpdateConcurrencyException("Concurrency conflict detected. The user might have been updated by someone else.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }
    }
}