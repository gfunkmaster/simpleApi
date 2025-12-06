using System.ComponentModel.DataAnnotations;
using Validations;

namespace SimpleApi.src.Models
{
    public class CreateCourseInstanceRequest
    {
        [Required(ErrorMessage = "Start date is required")]
        [FutureDate]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "End date is required")]
        [EndDateAfterStartDate]
        public DateTime EndDate { get; set; }
        
        [Required(ErrorMessage = "Course ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Course ID must be a positive number")]
        public int CourseId { get; set; }
        
        public List<int> StudentIds { get; set; } = new List<int>();
    }
}