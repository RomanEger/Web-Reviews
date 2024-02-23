using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataAnnotations
{
    public class PasswordValidation : ValidationAttribute
    {
        private bool ValidationUpperLetter(string value) =>
            value.Any(x => char.IsUpper(x));

        private bool ValidationLowerLetter(string value) =>
            value.Any(x => char.IsLower(x));

        private bool ValidationNumber(string value) =>
            value.Any(x => char.IsDigit(x));

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string errors = String.Empty;
            if (value is null)
                return new ValidationResult("Password is empty");

            if(!ValidationUpperLetter(value.ToString()))
                errors += "Password have to contain upper letter\n";

            if(!ValidationLowerLetter(value.ToString()))
                errors += "Password have to contain lower letter\n";

            if(!ValidationNumber(value.ToString()))
                errors += "Password have to contain number\n";

            return string.IsNullOrEmpty(errors)
                ? ValidationResult.Success
                : new ValidationResult(errors);
        }
    }
}
