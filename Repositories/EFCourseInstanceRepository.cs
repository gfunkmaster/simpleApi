using SimpleApi.src.Models;
using SimpleApi.Data;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SimpleApi.Repositories
{
    public class EFCourseInstanceRepository : ICourseInstanceRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCourseInstanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CourseInstance> GetAll()
        {
            return _context.CourseInstances
                .Include(ci => ci.Course) // Inkludera Course-information
                .Include(ci => ci.EnrolledStudents) // Inkludera enrolled students
                .ToList();
        }

        public CourseInstance? GetById(int id)
        {
            return _context.CourseInstances
                .Include(ci => ci.Course)
                .Include(ci => ci.EnrolledStudents) // Inkludera enrolled students
                .FirstOrDefault(ci => ci.Id == id);
        }

        public CourseInstance Add(CreateCourseInstanceRequest request)
        {
            // Kontrollera att kursen finns
            var course = _context.Courses.Find(request.CourseId);
            if (course == null) throw new ArgumentException("Course not found");

            var courseInstance = new CourseInstance
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CourseId = request.CourseId
            };

            _context.CourseInstances.Add(courseInstance);
            _context.SaveChanges();
            
            // Ladda relationen efter Save
            return _context.CourseInstances
                .Include(ci => ci.Course)
                .Include(ci => ci.EnrolledStudents)
                .First(ci => ci.Id == courseInstance.Id);
        }

        public CourseInstance? Update(int id, CreateCourseInstanceRequest updatedRequest)
        {
            var courseInstance = _context.CourseInstances.Find(id);
            if (courseInstance == null) return null;

            // Kontrollera att den nya kursen finns om CourseId ändras
            if (courseInstance.CourseId != updatedRequest.CourseId)
            {
                var newCourse = _context.Courses.Find(updatedRequest.CourseId);
                if (newCourse == null) return null;
                courseInstance.CourseId = updatedRequest.CourseId;
            }

            courseInstance.StartDate = updatedRequest.StartDate;
            courseInstance.EndDate = updatedRequest.EndDate;
            
            _context.SaveChanges();
            
            // Returnera med inkluderade relationer
            return _context.CourseInstances
                .Include(ci => ci.Course)
                .Include(ci => ci.EnrolledStudents)
                .First(ci => ci.Id == id);
        }

        public CourseInstance? Patch(int id, CreateCourseInstanceRequest patchRequest)
        {
            // Samma som Update för nu
            return Update(id, patchRequest);
        }

        public bool Delete(int id)
        {
            var courseInstance = _context.CourseInstances.Find(id);
            if (courseInstance == null) return false;

            _context.CourseInstances.Remove(courseInstance);
            _context.SaveChanges();
            return true;
        }

        public bool EnrollStudent(int courseInstanceId, int studentId)
        {
            var courseInstance = _context.CourseInstances
                .Include(ci => ci.EnrolledStudents)
                .FirstOrDefault(ci => ci.Id == courseInstanceId);
                
            var student = _context.Students.Find(studentId);
            
            if (courseInstance == null || student == null) return false;
            
            // Kontrollera att studenten inte redan är enrollad
            if (courseInstance.EnrolledStudents.Any(s => s.Id == studentId)) return false;
            
            courseInstance.EnrolledStudents.Add(student);
            _context.SaveChanges();
            return true;
        }

        public bool UnenrollStudent(int courseInstanceId, int studentId)
        {
            var courseInstance = _context.CourseInstances
                .Include(ci => ci.EnrolledStudents)
                .FirstOrDefault(ci => ci.Id == courseInstanceId);
                
            if (courseInstance == null) return false;
            
            var student = courseInstance.EnrolledStudents.FirstOrDefault(s => s.Id == studentId);
            if (student == null) return false;
            
            courseInstance.EnrolledStudents.Remove(student);
            _context.SaveChanges();
            return true;
        }
    }
}