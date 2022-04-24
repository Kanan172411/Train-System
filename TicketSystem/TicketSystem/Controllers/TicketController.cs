using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DAL;
using TicketSystem.Models;
using TicketSystem.ViewModels;

namespace TicketSystem.Controllers
{
    public class TicketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TicketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(DateTime? dateTime)
        {
            List<Ticket> tickets = _context.Tickets.Where(x=>x.IsDeleted==false).ToList();
            if (dateTime != null)
            {
                tickets = _context.Tickets.Where(x => x.DepartureTime == dateTime).ToList();
            }
            Settings settings = _context.Settings.FirstOrDefault();
            TicketViewModel ticketViewModel = new TicketViewModel
            {
                Tickets = tickets,
                settings = settings
            };
            return View(ticketViewModel);
        }
        [Authorize(Roles ="Member")]
        public async Task<IActionResult> Reservation(int Id)
        {
            ViewBag.Settings = _context.Settings.FirstOrDefault();
            Ticket ticket = _context.Tickets.FirstOrDefault(x => x.Id == Id);
            if (ticket == null || ticket.IsDeleted == true)
            {
                return NotFound();
            }
            ViewBag.TicketId = ticket.Id;
            return View();
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservation(TransactionViewModel transactionVM)
        {
            ViewBag.Settings = _context.Settings.FirstOrDefault();
            int Id = transactionVM.TicketId;
            Ticket ticket = _context.Tickets.FirstOrDefault(x => x.Id == Id);
            if (ticket == null || ticket.IsDeleted == true)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Reservation reservation = new Reservation()
            {
                AppUserId = user.Id,
                CreatedAt = DateTime.Now,
                TicketId = ticket.Id,
            };
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            Transaction transaction = new Transaction()
            {
                AppUserId = user.Id,
                ReservationId = reservation.Id,
                CustomerName = transactionVM.CustomerName,
                ExpirationTime = transactionVM.ExpirationTime,
                Number = transactionVM.CardNumber,
                CVV=transactionVM.CVV,
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return RedirectToAction("index","home");
        } 

    }
}
