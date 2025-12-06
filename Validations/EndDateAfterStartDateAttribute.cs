using System.ComponentModel.DataAnnotations;
using SimpleApi.src.Models;

namespace Validations
{
    public class EndDateAfterStartDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var request = validationContext.ObjectInstance as CreateCourseInstanceRequest;
            if (request != null && value is DateTime endDate)
            {
                if (endDate <= request.StartDate)
                {
                    return new ValidationResult("End date must be later than start date");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid date comparison");
        }
    }
}