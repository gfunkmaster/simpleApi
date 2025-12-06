using System.ComponentModel.DataAnnotations;

namespace SimpleApi.src.Models
{
    public class CreateCourseRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be at least 3 characters long")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
    }
}