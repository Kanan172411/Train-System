using TicketSystem.Models;

namespace TicketSystem.ViewModels
{
    public class TicketViewModel
    {
        public List<Ticket> Tickets { get; set; }
        public Settings settings { get; set; }
    }
}
