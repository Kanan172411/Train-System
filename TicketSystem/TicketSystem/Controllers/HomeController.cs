using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TicketSystem.DAL;
using TicketSystem.Models;
using TicketSystem.ViewModels;

namespace TicketSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Ticket> tickets = _context.Tickets.Where(x=>x.IsDeleted==false).Take(6).ToList();
            Settings settings = _context.Settings.FirstOrDefault();
            HomeViewModel homeViewModel = new HomeViewModel {
                Tickets1 = tickets.Take(3).ToList(),
                Tickets2 = tickets.Skip(3).Take(3).ToList(),
                settings = settings
            };
            return View(homeViewModel);
        }

    }
}