using LGC.Support.Models;
using LGC.Support.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace LGC.Support.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _user;
        public AccountController(UserService user)
        {
            _user = user;
        }
        public IActionResult Login()
        {
            var model = new UserData()
            {
                username = "xxx",
                password = "1234"
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(UserData model)
        {
            var _pass = EncDecHelper.EncryptData(model.password);
            var result = _user.LoginService(model.username,_pass).Result;
            if (result != null)
            {
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, result.username)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);
            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(3000)
            });

            return RedirectToAction("Index", "Home");
        }
            else
            {
                model.error_mess = "Invid Username / Password ";
                return View(model);
            }
        }
    }
}
