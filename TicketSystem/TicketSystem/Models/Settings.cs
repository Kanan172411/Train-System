using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class Settings
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Address1 { get; set; }       
        [Required]
        [StringLength(maximumLength: 150)]
        public string Address2 { get; set; }
        [Required]
        [StringLength(maximumLength: 60)]
        public string PhoneNumber1 { get; set; }
        [Required]
        [StringLength(maximumLength: 60)]
        public string PhoneNumber2 { get; set; }
        [Required]
        [StringLength(maximumLength: 70)]
        public string Email1 { get; set; }        
        [Required]
        [StringLength(maximumLength: 70)]
        public string Email2 { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string Facebook { get; set; }        
        [Required]
        [StringLength(maximumLength: 200)]
        public string Twitter { get; set; }        
        [Required]
        [StringLength(maximumLength: 200)]
        public string Linkedin { get; set; }       
        [Required]
        [StringLength(maximumLength: 200)]
        public string Instagram { get; set; }
    }
}
