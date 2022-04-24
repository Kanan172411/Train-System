using System.ComponentModel.DataAnnotations;

namespace TicketSystem.ViewModels
{
    public class TransactionViewModel
    {
        [Required]
        [StringLength(maximumLength:30)]
        public string CustomerName { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 16)]
        public string CardNumber { get; set; }
        [Required]
        public DateTime ExpirationTime { get; set; }
        [Required]
        public int CVV { get; set; }
        public int TicketId { get; set; }
    }
}
