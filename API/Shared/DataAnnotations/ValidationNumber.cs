using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataAnnotations
{
    public class ValidationNumber : ValidationAttribute
    {
        public string FieldName { get; set; } = "Поле";
        private bool ValidationNumberChar(string value) =>
           value.Any(x => char.IsDigit(x));

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            return ValidationNumberChar(value.ToString())
                ? ValidationResult.Success
                : new ValidationResult($"{FieldName} должен содержать цифру");
        }
    }
}
