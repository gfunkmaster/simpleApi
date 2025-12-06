using System.ComponentModel.DataAnnotations;

namespace Validations
{

    public class CustomEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string email)
            {
                if (!email.Contains("@"))
                {
                    return new ValidationResult("Email must contain @ symbol");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid email format");
        }
    }
}