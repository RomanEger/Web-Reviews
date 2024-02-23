﻿using Shared.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record UserForRegistrationDTO
    {
        [Required(ErrorMessage ="Nickname field is required")]
        [Length(4, 25,ErrorMessage ="Nickname length should be between 4 and 25 chars")]
        [SpecialSymbolValidation(ErrorMessage = "Nickname cant contain special symbols")]
        public string? Nickname { get; set; }

        [Required(ErrorMessage = "Email field is required")]
        [MaxLength(30, ErrorMessage ="Email should be less than 30 chars")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Nickname field is required")]
        [Length(8, 20, ErrorMessage = "Password length should be between 8 and 20 chars")]
        //[PasswordValidation]
        [ValidationLowerCase]
        [ValidationUpperCase]
        [ValidationNumber]
        [SpecialSymbolValidation(ErrorMessage = "Password cant contain special symbols")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "UserRankId field is required")]
        public Guid? UserRankId { get; set; }
    }
}
