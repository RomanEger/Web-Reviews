using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataAnnotations
{
    public class ValidationUpperCase : ValidationAttribute
    {
        public string FieldName { get; set; } = "Поле";
        private bool ValidationUpperLetter(string value) =>
            value.Any(x => char.IsUpper(x));

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            return ValidationUpperLetter(value.ToString())
                ? ValidationResult.Success
                : new ValidationResult($"{FieldName} должен содержать букву в верхнем регистре");
        }
    }
}
