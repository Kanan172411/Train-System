using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Areas.Manage.ViewModels
{
    public class AdminUpdateViewModel
    {
        public string UserName { get; set; }
        [StringLength(maximumLength: 20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(maximumLength: 20)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassowrd { get; set; }

        [StringLength(maximumLength: 20)]
        [DataType(DataType.Password)]
        public string CurrentPassowrd { get; set; }
    }
}
