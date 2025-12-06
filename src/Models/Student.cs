
namespace SimpleApi.src.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        // Parameterlös konstruktör för EF Core
        public Student() { }
        
        // Bekvämlighets-konstruktör för kod
        public Student(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}