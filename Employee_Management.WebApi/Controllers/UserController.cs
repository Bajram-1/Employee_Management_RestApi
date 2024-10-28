using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Employee_Management.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenService _jwtTokenService;

        public UsersController(IUserService userService, JwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            var user = await _userService.ValidateUser(login.UserName, login.Password); // Await the async method call
            if (user == null)
            {
                return Unauthorized();
            }

            var token = _jwtTokenService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}