using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataAnnotations
{
    public class ValidationLowerCase : ValidationAttribute
    {
        private bool ValidationLowerLetter(string value) =>
           value.Any(x => char.IsLower(x));

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            return ValidationLowerLetter(value.ToString())
                ? ValidationResult.Success
                : new ValidationResult("Field has to contain lower case letter");
        }
    }
}
