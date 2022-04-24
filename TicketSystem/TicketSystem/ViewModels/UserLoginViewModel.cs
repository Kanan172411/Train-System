using System.ComponentModel.DataAnnotations;

namespace TicketSystem.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        [StringLength(maximumLength: 25)]
        public string Username { get; set; }

        [Required]
        [StringLength(maximumLength: 20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
