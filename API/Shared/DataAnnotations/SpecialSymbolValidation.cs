using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataAnnotations
{
    public class SpecialSymbolValidation : ValidationAttribute
    {
        private bool ValidationStandart(string value) =>
            value.Count(x => char.IsLetterOrDigit(x)) == value.Length;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            return ValidationStandart(value.ToString())
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }
    }
}
