using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; } 
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int TicketId { get; set; }
        [Required]
        public string AppUserId { get; set; }
        public Ticket ticket { get; set; }
        public AppUser appUser { get; set; }

    }
}
