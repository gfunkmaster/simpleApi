using System.ComponentModel.DataAnnotations;

namespace Validations
{
    public class ValidGradeAttribute : ValidationAttribute
    {
        private readonly string[] _validGrades = { "A", "B", "C", "D", "E", "F" };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string grade)
            {
                if (!_validGrades.Contains(grade.ToUpper()))
                {
                    return new ValidationResult($"Grade must be one of: {string.Join(", ", _validGrades)}");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid grade format");
        }
    }
}