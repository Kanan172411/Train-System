using Microsoft.AspNetCore.Identity;

namespace TicketSystem.Models
{
    public class AppUser: IdentityUser
    {
        public bool IsAdmin { get; set; }
        public string FullName { get; set; }
        public List<Reservation> reservations { get; set; }
        public List<Transaction> transactions { get; set; }

    }
}
