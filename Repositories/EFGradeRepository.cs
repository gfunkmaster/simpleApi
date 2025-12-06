using SimpleApi.src.Models;
using SimpleApi.Data;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SimpleApi.Repositories
{
    public class EFGradeRepository : IGradeRepository
    {
        private readonly ApplicationDbContext _context;

        public EFGradeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Grade> GetAll()
        {
            return _context.Grades
                .Include(g => g.Student)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .ToList();
        }

        public Grade? GetById(int id)
        {
            return _context.Grades
                .Include(g => g.Student)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .FirstOrDefault(g => g.Id == id);
        }

        public Grade Add(CreateGradeRequest request)
        {
            // Kontrollera att Student finns
            var student = _context.Students.Find(request.StudentId);
            if (student == null) throw new ArgumentException("Student not found");

            // Kontrollera att CourseInstance finns
            var courseInstance = _context.CourseInstances.Find(request.CourseInstanceId);
            if (courseInstance == null) throw new ArgumentException("CourseInstance not found");

            var grade = new Grade
            {
                Value = request.Value,
                StudentId = request.StudentId,
                CourseInstanceId = request.CourseInstanceId
            };

            _context.Grades.Add(grade);
            _context.SaveChanges();
            
            // Returnera med inkluderade relationer
            return _context.Grades
                .Include(g => g.Student)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .First(g => g.Id == grade.Id);
        }

        public List<Grade> GetGradesByStudent(int studentId)
        {
            return _context.Grades
                .Include(g => g.Student)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .Where(g => g.Student.Id == studentId)
                .ToList();
        }

        public List<Grade> GetGradesByCourseInstance(int courseInstanceId)
        {
            return _context.Grades
                .Include(g => g.Student)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .Where(g => g.CourseInstance.Id == courseInstanceId)
                .ToList();
        }

        public bool Delete(int id)
        {
            var grade = _context.Grades.Find(id);
            if (grade == null) return false;

            _context.Grades.Remove(grade);
            _context.SaveChanges();
            return true;
        }
    }
}