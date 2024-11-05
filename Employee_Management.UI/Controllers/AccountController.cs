using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Employee_Management.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly JwtTokenService _jwtTokenService;

        public AccountController(IUserService userService, JwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userService.ValidateUser(model.UserName, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var token = _jwtTokenService.GenerateToken(user);
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login", "Account");
        }
    }
}
