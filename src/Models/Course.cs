
namespace SimpleApi.src.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Parameterlös konstruktör för EF Core
        public Course() { }
        
        // Bekvämlighets-konstruktör för kod
        public Course(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }
}