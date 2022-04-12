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
                    new Claim(ClaimTypes.Name, result.username),
                    new Claim("UserId", result.id.ToString()),
                    new Claim("UserFirstName", result.first_name),
                    new Claim("UserLastName", result.last_name)
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
                TempData["ErrorMessage"] = "Invalid Username / Password";
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
            try
            {
                if (model.password == model.confirm_password)
                {
                    var result = _user.RegisterService(model).Result;
                    if (result != null)
                    {
                        TempData["ErrorMessage"] = "Username is duplicate.";
                        return View(model);
                    }
                    else
                    {
                        TempData["Message"] = "Registered successfully.";
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Password doesn't match. Please try again.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
            }

        }

        public IActionResult Profile(int id)
        {
            var result = _user.Get(id).Result;
            return View(result);
        }

        [HttpPost]
        public IActionResult Profile(UserData model)
        {
            var result = _user.Update(model).Result;
            if (result != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult ChangePassword(int id)
        {
            var result = _user.Get(id).Result;
            return View(result);
        }

        [HttpPost]
        public IActionResult ChangePassword(UserData model)

        {
            if (model.new_password == model.confirm_password)
            {
                var result = _user.ChangePassword(model).Result;
                if (result != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Incorrect old password.";
                    return View(model);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "New password doesn't match. Please try again.";
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

