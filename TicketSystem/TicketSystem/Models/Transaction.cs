using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class Transaction
    {
        public int Id { get; set; } 
        
        public string AppUserId { get; set; }
        public int ReservationId { get; set; }
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public DateTime ExpirationTime { get; set; }
        [Required]
        public int CVV { get; set; }
        public AppUser appUser { get; set; }
    }
}
