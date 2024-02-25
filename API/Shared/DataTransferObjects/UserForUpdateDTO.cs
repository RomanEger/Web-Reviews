﻿using Entities.Models;
using Shared.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record UserForUpdateDTO
    {
        [Required(ErrorMessage = "Никнейм поле обязательно")]
        [Length(4, 25, ErrorMessage = "Длина ника должна быть между 4 и 25 символами")]
        [SpecialSymbolValidation(ErrorMessage = "Никнейм не может содержать специальных символов")]
        public string? Nickname { get; init; }


        [Required(ErrorMessage = "Пароль обязательное поле")]
        [Length(8, 20, ErrorMessage = "Длина пароля должна быть между 8 и 25 символами")]
        [ValidationLowerCase(FieldName = "Пароль")]
        [ValidationUpperCase(FieldName = "Пароль")]
        [ValidationNumber(FieldName = "Пароль")]
        [SpecialSymbolValidation(ErrorMessage = "Пароль не может содержать специальных символов")]
        public string? Password { get; set; }


        [Required(ErrorMessage = "UserRankId поле обязательно")]
        public Guid? UserRankId { get; init; }


        public byte[]? Photo { get; init; }
    }
}
