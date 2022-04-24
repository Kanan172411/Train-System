using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int TicketNumber { get; set; }
        [Range(1,1000)]
        public double Price { get; set; }
        [Required]
        public string TrainNumber { get; set; }
        [StringLength(maximumLength: 70)]
        public string From { get; set; }       
        [StringLength(maximumLength: 70)]
        public string To { get; set; }
        [StringLength(maximumLength:70)]
        public string? ImageName { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public DateTime ArrivingTime { get; set; }
        public bool IsDeleted { get; set; }
        public List<Reservation> Reservations { get; set; }

    }
}
