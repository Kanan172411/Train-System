using Microsoft.AspNetCore.Mvc;
using TicketSystem.Models;
using TicketSystem.DAL;
using Microsoft.AspNetCore.Authorization;

namespace TicketSystem.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Manage")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            Settings settings = _context.Settings.FirstOrDefault();
            return View(settings);
        }
        public IActionResult Edit(int id)
        {

            Settings setting = _context.Settings.FirstOrDefault(x => x.Id == id);

            if (setting == null) return RedirectToAction("error", "dashboard");

            return View(setting);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Settings setting)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Settings existsetting = _context.Settings.FirstOrDefault(x => x.Id == setting.Id);

            if (existsetting == null) return RedirectToAction("error", "dashboard");

            existsetting.Address1 = setting.Address1;
            existsetting.Address2 = setting.Address2;
            existsetting.PhoneNumber1 = setting.PhoneNumber1;
            existsetting.PhoneNumber2 = setting.PhoneNumber2;
            existsetting.Email1 = setting.Email1;
            existsetting.Email2 = setting.Email2;
            existsetting.Facebook = setting.Facebook;
            existsetting.Twitter = setting.Twitter;
            existsetting.Instagram = setting.Instagram;
            existsetting.Linkedin = setting.Linkedin;
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
    }
