using TicketSystem.Models;

namespace TicketSystem.ViewModels
{
    public class MyReservationViewModel
    {
        public List<Ticket> tickets { get; set; }
        public Settings settings { get; set; }
    }
}
