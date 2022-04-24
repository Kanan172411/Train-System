using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL;
using TicketSystem.Models;

namespace TicketSystem.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class TransactionController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Transaction> transactions = _context.Transactions.Include(x=>x.appUser).ToList();
            return View(transactions);
        }
    }
}
