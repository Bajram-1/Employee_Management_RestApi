using Employee_Management.BLL.DTO;
using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Employee_Management.BLL.Services;
using Employee_Management.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Employee_Management.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return Ok("Create action of UserApiController");
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] UserCreateViewModel model)
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
                var user = new User
                {
                    UserName = model.UserName,
                    PasswordHash = hashedPassword,
                    Email = model.Email,
                    Role = model.Role,
                    City = model.City,
                    PhoneNumber = model.PhoneNumber,
                    ProfilePicture = profilePicturePath
                };
                await _userService.AddUserAsync(user);
                return Ok(user);
            }
            return BadRequest(ModelState);
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

        [HttpGet("edit/{id}")]
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
            return Ok(model);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid)
            {
                string profilePicturePath = model.ProfilePicture;
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
                    PasswordHash = model.Password,
                    Role = model.Role,
                    City = model.City,
                    PhoneNumber = model.PhoneNumber,
                    ProfilePicture = profilePicturePath
                };
                await _userService.UpdateUserAsync(dtoUser);
                return Ok(dtoUser);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet("profile/{id}")]
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
            return Ok(model);
        }
    }
}