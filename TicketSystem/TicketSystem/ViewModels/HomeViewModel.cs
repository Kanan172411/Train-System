
using TicketSystem.Models;

namespace TicketSystem.ViewModels
{
    public class HomeViewModel
    {
        public List<Ticket> Tickets1 { get; set; }   
        public List<Ticket> Tickets2 { get; set; }   
        public Settings settings { get; set; }
    }
}
