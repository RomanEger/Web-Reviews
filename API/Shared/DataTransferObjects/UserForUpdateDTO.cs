﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public class UserForUpdateDTO
    {
        [Required(ErrorMessage = "Nickname field is required")]
        [Length(4, 25, ErrorMessage = "Nickname length should be between 4 and 25 chars")]
        public string? Nickname { get; set; }


        [Required(ErrorMessage = "Nickname field is required")]
        [Length(8, 20, ErrorMessage = "Password length should be between 8 and 20 chars")]
        public string? Password { get; set; }


        [Required(ErrorMessage = "UserRankId field is required")]
        public Guid? UserRankId { get; set; }


        public byte[]? Photo { get; set; }
    }
}
