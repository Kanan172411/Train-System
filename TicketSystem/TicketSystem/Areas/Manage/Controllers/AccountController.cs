using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Areas.Manage.ViewModels;
using TicketSystem.Models;

namespace TicketSystem.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser admin = await _userManager.FindByNameAsync(loginVM.UserName);

            if (admin == null || !admin.IsAdmin)
            {
                ModelState.AddModelError("", "UserName ve ya sifre yanlsidir!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(admin, loginVM.Password, true, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName ve ya sifre yanlsidir!");
                return View();
            }

            return RedirectToAction("index", "dashboard");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("login", "account");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            AdminUpdateViewModel updateModel = new AdminUpdateViewModel
            {
                UserName = user.UserName
            };

            return View(updateModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminUpdateViewModel updateVM)
        {
            if (updateVM.UserName == null)
            {
                ModelState.AddModelError("UserName", "The UserName field is required.!");
                return View();
            }

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user.UserName != updateVM.UserName && _userManager.Users.Any(x => x.NormalizedUserName == updateVM.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "UserName already taken");
                return View();
            }

            if (!string.IsNullOrWhiteSpace(updateVM.Password) || !string.IsNullOrWhiteSpace(updateVM.CurrentPassowrd) || !string.IsNullOrWhiteSpace(updateVM.ConfirmPassowrd)) 
            {
                if (updateVM.Password != updateVM.ConfirmPassowrd)
                {
                    ModelState.AddModelError("ConfirmPassowrd", "Password ve Confirm password eyni olmalidir!");
                    return View();
                }
                if (!string.IsNullOrWhiteSpace(updateVM.CurrentPassowrd))
                {
                    ModelState.AddModelError("CurrentPassword", "The CurrentPassowrd field is required.!");
                    return View();
                }      
                if (!string.IsNullOrWhiteSpace(updateVM.Password))
                {
                    ModelState.AddModelError("Password", "The CurrentPassowrd field is required.!");
                    return View();
                }

                var result = await _userManager.ChangePasswordAsync(user, updateVM.CurrentPassowrd, updateVM.Password);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View();
                }
            }
            user.UserName = updateVM.UserName;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("index", "dashboard");
        }
    }
}
