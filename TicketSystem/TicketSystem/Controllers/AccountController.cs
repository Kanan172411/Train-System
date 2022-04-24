using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL;
using TicketSystem.Models;
using TicketSystem.ViewModels;

namespace TicketSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            ViewBag.Settings = _context.Settings.FirstOrDefault();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel registerVM)
        {
            ViewBag.Settings = _context.Settings.FirstOrDefault();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_userManager.Users.Any(x => x.NormalizedUserName == registerVM.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "UserName already taken!");
                return View();
            }

            if (_userManager.Users.Any(x => x.PhoneNumber == registerVM.PhoneNumber.ToUpper()))
            {
                ModelState.AddModelError("PhoneNumber", "PhoneNumber already taken!");
                return View();
            }


            AppUser appUser = new AppUser
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                PhoneNumber = registerVM.PhoneNumber,
                IsAdmin = false,
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(appUser, "Member");
            await _signInManager.SignInAsync(appUser, true);
            return RedirectToAction("index", "home");
        }

        public IActionResult Login()
        {
            ViewBag.Settings = _context.Settings.FirstOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel loginVM)
        {
            ViewBag.Settings = _context.Settings.FirstOrDefault();

            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(loginVM.Username);

            if (user == null || user.IsAdmin)
            {
                ModelState.AddModelError("", "UserName ve ya Sifre yanlisdir!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, true, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName ve ya Sifre yanlisdir!");
                return View();
            }

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Edit()
        {
            ViewBag.Settings = _context.Settings.FirstOrDefault();
            try
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user.IsAdmin == true)
                {
                    return RedirectToAction("Login");
                }
                UserUpdateViewModel updateModel = new UserUpdateViewModel
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,

                };
                return View(updateModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Edit(UserUpdateViewModel updateVM)
        {
            ViewBag.Settings = _context.Settings.FirstOrDefault();
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user.UserName != updateVM.UserName && _userManager.Users.Any(x => x.NormalizedUserName == updateVM.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "UserName already taken");
                return View();
            }

            if (user.PhoneNumber != updateVM.PhoneNumber && _userManager.Users.Any(x => x.PhoneNumber == updateVM.PhoneNumber.ToUpper()))
            {
                ModelState.AddModelError("PhoneNumber", "PhoneNumber already taken");
                return View();
            }

           

            user.UserName = updateVM.UserName;
            user.FullName = updateVM.FullName;
            user.PhoneNumber = updateVM.PhoneNumber;

            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, true);


            return RedirectToAction("index", "home");
        }
        [Authorize(Roles = "Member")]
        public IActionResult ChangePassword()
        {
            ViewBag.Settings = _context.Settings.FirstOrDefault();
            if (User.IsInRole("Member"))
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordVM)
        {
            ViewBag.Settings = _context.Settings.FirstOrDefault();
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!string.IsNullOrWhiteSpace(changePasswordVM.Password))
            {
                if (changePasswordVM.Password != changePasswordVM.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassowrd", "Password ve Confirm password eyni olmalidir!");
                    return View();
                }
                if (changePasswordVM.CurrentPassword == null)
                {
                    ModelState.AddModelError("CurrentPassword", "This field is required");
                    return View();
                }
                var result = await _userManager.ChangePasswordAsync(user, changePasswordVM.CurrentPassword, changePasswordVM.Password);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View();
                }

            }
            else
            {
                ModelState.AddModelError("", "All fields are required");
                return View();
            }
            return RedirectToAction("index", "home");
        }
        [Authorize(Roles ="Member")]
        public async Task<IActionResult> MyReservation()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Reservation> reservations = _context.Reservations.Include(x=>x.ticket).Where(x=>x.appUser.Id == user.Id).ToList();
            List<int> TicketIds = new List<int>();
            foreach(var item in reservations)
            {
                TicketIds.Add(item.ticket.Id);
            }
            List<Ticket> Tickets = new List<Ticket>();
            foreach(var item in _context.Tickets)
            {
                foreach(var item1 in TicketIds)
                {
                    if (item.Id == item1)
                    {
                        Tickets.Add(item);
                    }
                }
            }
            MyReservationViewModel myReservationViewModel = new MyReservationViewModel()
            {
                settings = _context.Settings.FirstOrDefault(),
                tickets = Tickets,
            };
            return View(myReservationViewModel);
        }
    }
}
