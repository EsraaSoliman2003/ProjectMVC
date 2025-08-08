using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AppDbContext context;

        public AccountController(AppDbContext Context)
        {
            context = Context;
        }

        // ===================== التفاصيل =====================
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UsersWithRoles()
        {
            var users = context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    Roles = context.UserRoles
                                .Where(ur => ur.UserId == u.Id)
                                .Select(ur => ur.Role.Name)
                                .ToList()
                })
                .ToList();

            return View(users);
        }

        // ===================== التسجيل =====================
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            ViewBag.Roles = context.Roles.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Register(User user, List<int> roleIds)
        {
            if (ModelState.IsValid)
            {
                context.Users.Add(user);
                context.SaveChanges();

                foreach (var roleId in roleIds)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId
                    };
                    context.UserRoles.Add(userRole);
                }

                context.SaveChanges();

                return RedirectToAction("UsersWithRoles");
            }

            ViewBag.Roles = context.Roles.ToList();
            return View(user);
        }

        // ===================== تسجيل الدخول =====================
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string username, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var roles = context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name)
                .ToList();

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)).GetAwaiter().GetResult();

            return RedirectToAction("Index", "Departments");
        }

        // ===================== تسجيل الخروج =====================
        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).GetAwaiter().GetResult();
            return RedirectToAction("Login");
        }

        // ===================== عدم السماح =====================
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
