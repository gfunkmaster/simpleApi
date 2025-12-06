using System.ComponentModel.DataAnnotations;
using Validations;

namespace SimpleApi.src.Models
{
    public class CreateStudentRequest(string name, string email)
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = name;
        [CustomEmail]
        public string Email { get; set; } = email;
    }
}