using SimpleApi.src.Models;
using SimpleApi.Data;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SimpleApi.Repositories
{
    public class EFGradeRepository(ApplicationDbContext context) : IGradeRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Grade>> GetAllAsync()
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .ToListAsync();
        }

        public async Task<Grade?> GetByIdAsync(int id)
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Grade> AddAsync(CreateGradeRequest request)
        {
            var student = await _context.Students.FindAsync(request.StudentId);
            if (student == null) throw new ArgumentException("Student not found");

            var courseInstance = await _context.CourseInstances.FindAsync(request.CourseInstanceId);
            if (courseInstance == null) throw new ArgumentException("CourseInstance not found");

            var grade = new Grade
            {
                Value = request.Value,
                StudentId = request.StudentId,
                CourseInstanceId = request.CourseInstanceId
            };

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .FirstAsync(g => g.Id == grade.Id);
        }

        public async Task<List<Grade>> GetGradesByStudentAsync(int studentId)
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .Where(g => g.Student.Id == studentId)
                .ToListAsync();
        }

        public async Task<List<Grade>> GetGradesByCourseInstanceAsync(int courseInstanceId)
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.CourseInstance)
                    .ThenInclude(ci => ci.Course)
                .Where(g => g.CourseInstance.Id == courseInstanceId)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return false;

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}