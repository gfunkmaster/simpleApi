namespace SimpleApi.src.Models
{
    public class CourseInstance
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        // Navigation properties
        public Course Course { get; set; } = null!;
        public List<Student> EnrolledStudents { get; set; } = new();
        
        // Foreign key for EF Core
        public int CourseId { get; set; }
        
        // Parameterlös konstruktör för EF Core
        public CourseInstance() { }
        
        // Bekvämlighets-konstruktör för kod
        public CourseInstance(int id, DateTime startDate, DateTime endDate, Course course, List<Student> enrolledStudents)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            Course = course;
            CourseId = course.Id;
            EnrolledStudents = enrolledStudents;
        }
    }
}