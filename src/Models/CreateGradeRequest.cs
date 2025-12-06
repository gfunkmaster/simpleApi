using System.ComponentModel.DataAnnotations;
using Validations;

namespace SimpleApi.src.Models
{
    public class CreateGradeRequest
    {
        [Required(ErrorMessage = "Grade value is required")]
        [ValidGrade]
        public string Value { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Course instance ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Course instance ID must be a positive number")]
        public int CourseInstanceId { get; set; }
        
        [Required(ErrorMessage = "Student ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Student ID must be a positive number")]
        public int StudentId { get; set; }
    }
}