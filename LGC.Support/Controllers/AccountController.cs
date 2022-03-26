using LGC.Support.Models;
using LGC.Support.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace LGC.Support.Controllers
{
    [AllowAnonymous]
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
                username = "",
                password = ""
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(UserData model)
        {
            var result = _user.LoginService(model).Result;
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

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserData model)
        {
            if (model.password == model.confirm_password)
            {
                var result = _user.RegisterService(model).Result;
                if (result != null)
                {
                    model.error_mess = "Can't create new user account. Username already exists.";
                    return View(model);
                }
                else
                {
                    TempData["success"] = "Registered successfully.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                model.error_mess = "Password doesn't match. Please try again.";
                return View(model);
            }
        }

        public IActionResult Profile(int? id)
        {
            var result = _user.Get(id).Result;

            if (result == null)
            {   
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult Profile(UserData model)
        {
            if (model.password == model.confirm_password)
            {
                var result = _user.RegisterService(model).Result;
                if (result != null)
                {
                    model.error_mess = "Can't create new user account. Username already exists.";
                    return View(model);
                }
                else
                {
                    TempData["success"] = "Registered successfully.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                model.error_mess = "Password doesn't match. Please try again.";
                return View(model);
            }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}

