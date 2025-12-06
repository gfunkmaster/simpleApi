using System.ComponentModel.DataAnnotations;

namespace Validations
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date < DateTime.Now.Date)
                {
                    return new ValidationResult("Start date cannot be in the past");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid date format");
        }
    }
}