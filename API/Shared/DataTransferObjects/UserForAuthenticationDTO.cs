using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record UserForAuthenticationDTO
    {
        [Required(ErrorMessage = "Nickname field is required")]
        public string? UserPersonalData { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        public string? Password { get; set; }
    }
}
