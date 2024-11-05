using Employee_Management.BLL.DTO;
using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Employee_Management.Common;
using Employee_Management.Common.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Employee_Management.UI.Controllers
{
    //[Authorize(Roles = StaticDetails.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = HashPassword(model.Password);
                string profilePicturePath = null;

                if (model.ProfilePictureFile != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = $"{model.UserName}_{Path.GetFileName(model.ProfilePictureFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePictureFile.CopyToAsync(fileStream);
                    }
                    profilePicturePath = $"/images/profiles/{fileName}";
                }

                var user = new BLL.DTO.User
                {
                    UserName = model.UserName,
                    PasswordHash = hashedPassword,
                    Email = model.Email,
                    EmailConfirmed = true,
                    Role = model.Role,
                    City = model.City,
                    PhoneNumber = model.PhoneNumber,
                    ProfilePicture = profilePicturePath,
                };

                await _userService.AddUserAsync(user);

                return RedirectToAction("Index", "User");
            }
            return View(model);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var byteValue in bytes)
                {
                    builder.Append(byteValue.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var model = new UserUpdateViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.PasswordHash,
                Role = Enum.Parse<UserRole>(user.Role),
                City = user.City,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model);
            }

            var existingUser = await _userService.GetUserByIdAsync(model.Id);
            if (existingUser == null) return NotFound();

            string profilePicturePath = existingUser.ProfilePicture;

            if (model.ProfilePictureFile != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{model.UserName}_{Path.GetFileName(model.ProfilePictureFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePictureFile.CopyToAsync(fileStream);
                }
                profilePicturePath = $"/images/profiles/{fileName}";
            }

            var dtoUser = new User
            {
                Id = model.Id,
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = !string.IsNullOrWhiteSpace(model.Password) ? model.Password : existingUser.PasswordHash,
                Role = model.Role,
                City = model.City,
                PhoneNumber = model.PhoneNumber,
                ProfilePicture = profilePicturePath
            };

            await _userService.UpdateUserAsync(dtoUser);
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserProfileViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                City = user.City,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture
            };

            return View(model);
        }
    }
}