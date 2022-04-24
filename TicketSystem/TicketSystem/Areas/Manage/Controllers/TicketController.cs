using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DAL;
using TicketSystem.Helpers;
using TicketSystem.Models;

namespace TicketSystem.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Manage")]

    public class TicketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TicketController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Ticket> tickets = _context.Tickets.ToList();
            return View(tickets);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ticket ticket)
        {
            if (_context.Tickets.Any(x => x.TicketNumber == ticket.TicketNumber)) 
            {
                ModelState.AddModelError("TicketNumber", "TicketNumber must be unique");
                return View(ticket);
            }
            if (ticket.ImageFile != null)
            {
                if (ticket.ImageFile.ContentType != "image/jpeg" && ticket.ImageFile.ContentType != "image/png" && ticket.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("ImageFile", "Fayl   .jpg, webp ve ya   .png ola biler!");
                    return View();
                }

                if (ticket.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "Fayl olcusu 3mb-dan boyuk ola bilmez!");
                    return View();
                }

                ticket.ImageName = FileManager.Save(_env.WebRootPath, "assets/photos", ticket.ImageFile);
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Image yuklemek mecburidir!");
                return View();
            }

            _context.Tickets.Add(ticket);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Ticket ticket = _context.Tickets.FirstOrDefault(x => x.Id == id);

            if (ticket == null) return RedirectToAction("error", "dashboard");

            return View(ticket);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ticket ticket)
        {

            Ticket existticket = _context.Tickets.FirstOrDefault(x => x.Id == ticket.Id);

            if (existticket == null) return RedirectToAction("error", "dashboard");
            if (existticket.Id!=ticket.Id)
            {
                if (_context.Tickets.Any(x => x.TicketNumber == ticket.TicketNumber))
                {
                    ModelState.AddModelError("TicketNumber", "TicketNumber must be unique");
                    return View(ticket);
                }
            }
            if (ticket.ImageFile != null)
            {
                if (ticket.ImageFile.ContentType != "image/jpeg" && ticket.ImageFile.ContentType != "image/png" && ticket.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("ImageFile", "Fayl   .jpg, webp ve ya   .png ola biler!");
                    return View();
                }

                if (ticket.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "Fayl olcusu 3mb-dan boyuk ola bilmez!");
                    return View();
                }


                string newFileName = FileManager.Save(_env.WebRootPath, "assets/photos", ticket.ImageFile);

                if (!string.IsNullOrWhiteSpace(existticket.ImageName))
                {
                    string oldFilePath = Path.Combine(_env.WebRootPath, "assets/photos", existticket.ImageName);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                existticket.ImageName = newFileName;
            }
            existticket.TicketNumber = ticket.TicketNumber;
            existticket.Price = ticket.Price;
            existticket.DepartureTime = ticket.DepartureTime;
            existticket.ArrivingTime = ticket.ArrivingTime;
            existticket.From = ticket.From;
            existticket.To = ticket.To;
            existticket.TrainNumber = ticket.TrainNumber;
            existticket.IsDeleted  = ticket.IsDeleted;
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Ticket existticket = _context.Tickets.FirstOrDefault(x => x.Id == id);
            if (existticket == null)
            {
                return RedirectToAction("error", "dashboard");
            }

            if (!string.IsNullOrWhiteSpace(existticket.ImageName))
            {
                FileManager.Delete(_env.WebRootPath, "assets/photos", existticket.ImageName);
            }
            _context.Tickets.Remove(existticket);
            _context.SaveChanges();

            return Json(new { status = 200 });
        }
    }
}
