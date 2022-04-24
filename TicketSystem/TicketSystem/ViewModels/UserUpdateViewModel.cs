using System.ComponentModel.DataAnnotations;

namespace TicketSystem.ViewModels
{
    public class UserUpdateViewModel
    {
        [Required]
        [StringLength(maximumLength: 25)]
        public string UserName { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string FullName { get; set; }
    }
}
