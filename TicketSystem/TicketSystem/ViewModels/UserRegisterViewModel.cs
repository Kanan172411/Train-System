using System.ComponentModel.DataAnnotations;

namespace TicketSystem.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        [StringLength(maximumLength: 80)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        public string UserName { get; set; }
        [Required]
        [StringLength (maximumLength: 50)]
        public string PhoneNumber { get; set; }

        [StringLength(maximumLength: 20)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [StringLength(maximumLength: 20)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
