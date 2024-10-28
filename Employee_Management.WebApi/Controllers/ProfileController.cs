using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("ViewProfilePicture")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity.Name;
            var user = await _context.Users.Include(u => u.Profile).SingleOrDefaultAsync(u => u.UserName == username);

            if (user == null) return NotFound();

            return Ok(user.Profile);
        }

        [HttpPut("EditProfilePicture")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromForm] UserProfileUpdateViewModel model)
        {
            var username = User.Identity.Name;
            var user = await _context.Users.Include(u => u.Profile).SingleOrDefaultAsync(u => u.UserName == username);

            if (user == null) return NotFound();

            user.Profile.FirstName = model.FirstName;
            user.Profile.LastName = model.LastName;

            if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfilePicture.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }
                user.Profile.ProfilePictureUrl = $"/uploads/{fileName}";
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}