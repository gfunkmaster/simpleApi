namespace SimpleApi.src.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
        
        // Navigation properties
        public CourseInstance CourseInstance { get; set; } = null!;
        public Student Student { get; set; } = null!;
        
        // Foreign keys for EF Core
        public int CourseInstanceId { get; set; }
        public int StudentId { get; set; }
        
        // Parameterlös konstruktör för EF Core
        public Grade() { }
        
        // Bekvämlighets-konstruktör för kod
        public Grade(int id, string value, CourseInstance courseInstance, Student student)
        {
            Id = id;
            Value = value;
            CourseInstance = courseInstance;
            CourseInstanceId = courseInstance.Id;
            Student = student;
            StudentId = student.Id;
        }
    }
}