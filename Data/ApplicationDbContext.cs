using Microsoft.EntityFrameworkCore;
using SimpleApi.src.Models;

namespace SimpleApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // Initialize the database
            Database.EnsureCreated();
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseInstance> CourseInstances { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<CourseInstance>()
                .HasOne(ci => ci.Course)
                .WithMany()
                .HasForeignKey(ci => ci.CourseId);

            // Configure Many-to-Many relationship: CourseInstance <-> Students
            modelBuilder.Entity<CourseInstance>()
                .HasMany(ci => ci.EnrolledStudents)
                .WithMany();

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany()
                .HasForeignKey(g => g.StudentId);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.CourseInstance)
                .WithMany()
                .HasForeignKey(g => g.CourseInstanceId);

            // Seed data för Students
            modelBuilder.Entity<Student>().HasData(
                new Student(1, "John Doe", "john@example.com"),
                new Student(2, "Jane Smith", "jane@example.com"),
                new Student(3, "Bob Johnson", "bob@example.com")
            );

            // Seed data för Courses  
            modelBuilder.Entity<Course>().HasData(
                new Course(1, "Programming", "Learn programming"),
                new Course(2, "Math", "Mathematics course")
            );

            // Seed data för CourseInstances
            modelBuilder.Entity<CourseInstance>().HasData(
                new CourseInstance { Id = 1, StartDate = DateTime.Now.AddDays(7), EndDate = DateTime.Now.AddDays(30), CourseId = 1 },
                new CourseInstance { Id = 2, StartDate = DateTime.Now.AddDays(14), EndDate = DateTime.Now.AddDays(45), CourseId = 2 }
            );

            // Seed data för Grades
            modelBuilder.Entity<Grade>().HasData(
                new Grade { Id = 1, Value = "A", StudentId = 1, CourseInstanceId = 1 },
                new Grade { Id = 2, Value = "B", StudentId = 2, CourseInstanceId = 1 },
                new Grade { Id = 3, Value = "C", StudentId = 3, CourseInstanceId = 2 }
            );

            // Many-to-Many seed data behöver läggas till manuellt efter appstart
            // eller via en endpoint för att enrolla studenter
        }
    }
}