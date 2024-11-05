using Employee_Management.Common;
using Employee_Management.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.DAL.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            ApplicationDbContext db,
            ILogger<DbInitializer> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    await _db.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying migrations.");
            }

            if (!await _roleManager.RoleExistsAsync(StaticDetails.Role_Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole<int> { Name = StaticDetails.Role_Employee });
                await _roleManager.CreateAsync(new IdentityRole<int> { Name = StaticDetails.Role_Admin });

                var user = new User
                {
                    UserName = "bani",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    PasswordHash = "Albania128.",
                    Role = "Admin",
                    PhoneNumber = "0695668776",
                    StreetAddress = "Rruga Arben Lami",
                    City = "Tirana"
                };

                var userCreationResult = await _userManager.CreateAsync(user, "Albania128.");
                if (!userCreationResult.Succeeded)
                {
                    _logger.LogError("Failed to create default admin user. Errors: {Errors}", string.Join(", ", userCreationResult.Errors.Select(e => e.Description)));
                    return;
                }

                var createdUser = await _userManager.FindByEmailAsync(user.Email);
                if (createdUser == null)
                {
                    _logger.LogError("Failed to find the user after creation.");
                    return;
                }

                var roleResult = await _userManager.AddToRoleAsync(createdUser, StaticDetails.Role_Admin);
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("Failed to assign admin role to the user. Errors: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
                else
                {
                    _logger.LogInformation("Admin role assigned to user successfully.");
                }
            }
        }
    }
}
