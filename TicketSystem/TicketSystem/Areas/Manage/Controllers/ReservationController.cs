using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL;
using TicketSystem.Models;

namespace TicketSystem.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Reservation> reservations = _context.Reservations.Include(x=>x.appUser).Include(x=>x.ticket).ToList();
            return View(reservations);
        }
    }
}
