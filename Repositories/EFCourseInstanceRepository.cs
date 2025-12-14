using SimpleApi.src.Models;
using SimpleApi.Data;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SimpleApi.Repositories
{
    public class EFCourseInstanceRepository(ApplicationDbContext context) : ICourseInstanceRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<CourseInstance>> GetAllAsync()
        {
            return await _context.CourseInstances
                .Include(ci => ci.Course)
                .Include(ci => ci.EnrolledStudents)
                .ToListAsync();
        }

        public async Task<CourseInstance?> GetByIdAsync(int id)
        {
            return await _context.CourseInstances
                .Include(ci => ci.Course)
                .Include(ci => ci.EnrolledStudents)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<CourseInstance> AddAsync(CreateCourseInstanceRequest request)
        {
            var course = await _context.Courses.FindAsync(request.CourseId);
            if (course == null) throw new ArgumentException("Course not found");

            var courseInstance = new CourseInstance
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CourseId = request.CourseId
            };

            _context.CourseInstances.Add(courseInstance);
            await _context.SaveChangesAsync();

            return await _context.CourseInstances
                .Include(ci => ci.Course)
                .Include(ci => ci.EnrolledStudents)
                .FirstAsync(ci => ci.Id == courseInstance.Id);
        }

        public async Task<CourseInstance?> UpdateAsync(int id, CreateCourseInstanceRequest updatedRequest)
        {
            var courseInstance = await _context.CourseInstances.FindAsync(id);
            if (courseInstance == null) return null;

            if (courseInstance.CourseId != updatedRequest.CourseId)
            {
                var newCourse = await _context.Courses.FindAsync(updatedRequest.CourseId);
                if (newCourse == null) return null;
                courseInstance.CourseId = updatedRequest.CourseId;
            }

            courseInstance.StartDate = updatedRequest.StartDate;
            courseInstance.EndDate = updatedRequest.EndDate;

            await _context.SaveChangesAsync();

            return await _context.CourseInstances
                .Include(ci => ci.Course)
                .Include(ci => ci.EnrolledStudents)
                .FirstAsync(ci => ci.Id == id);
        }

        public async Task<CourseInstance?> PatchAsync(int id, CreateCourseInstanceRequest patchRequest)
        {
            return await UpdateAsync(id, patchRequest);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var courseInstance = await _context.CourseInstances.FindAsync(id);
            if (courseInstance == null) return false;

            _context.CourseInstances.Remove(courseInstance);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EnrollStudentAsync(int courseInstanceId, int studentId)
        {
            var courseInstance = await _context.CourseInstances
                .Include(ci => ci.EnrolledStudents)
                .FirstOrDefaultAsync(ci => ci.Id == courseInstanceId);

            var student = await _context.Students.FindAsync(studentId);

            if (courseInstance == null || student == null) return false;

            if (courseInstance.EnrolledStudents.Any(s => s.Id == studentId)) return false;

            courseInstance.EnrolledStudents.Add(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnenrollStudentAsync(int courseInstanceId, int studentId)
        {
            var courseInstance = await _context.CourseInstances
                .Include(ci => ci.EnrolledStudents)
                .FirstOrDefaultAsync(ci => ci.Id == courseInstanceId);

            if (courseInstance == null) return false;

            var student = courseInstance.EnrolledStudents.FirstOrDefault(s => s.Id == studentId);
            if (student == null) return false;

            courseInstance.EnrolledStudents.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}