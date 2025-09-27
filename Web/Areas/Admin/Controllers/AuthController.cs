using BlogWebApp.Entity.DTOs.Users;
using BlogWebApp.Entity.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Web.Consts;
namespace Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AuthController:Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager; 
        public AuthController(SignInManager<AppUser> _signInManager, UserManager<AppUser> _userManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(userLoginDto.Email);
                if(user != null)
                {

                    var roles = await userManager.GetRolesAsync(user);

                    if (roles.Contains(RoleConsts.User) && !user.EmailConfirmed)
                    {
                        ModelState.AddModelError("", "E-posta adresinizi doğrulamanız gerekmektedir.");
                        return View(userLoginDto);
                    }

                    var result = await signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false);

                    if (result.Succeeded)
                    {

                        TempData["SuccessMessage"] = "Giriş başarılı! Yönlendiriliyorsunuz...";
                        return RedirectToAction("Index", "Home", new { Area = "" });

                    }
                    else
                    {
                        ModelState.AddModelError("","E-posta adresiniz veya şifreniz hatalıdır.");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError("", "E-posta adresiniz veya şifreniz hatalıdır.");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });

        }



        [Authorize]
        [HttpGet]
        public  IActionResult AccessDenied()
        {
            return View();
        }



    }
}
