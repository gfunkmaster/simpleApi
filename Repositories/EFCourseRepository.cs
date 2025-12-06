using SimpleApi.src.Models;
using SimpleApi.Data;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SimpleApi.Repositories
{
    public class EFCourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<Course> AddAsync(CreateCourseRequest request)
        {
            var course = new Course
            {
                Title = request.Title,
                Description = request.Description
            };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> UpdateAsync(int id, CreateCourseRequest updatedRequest)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return null;

            course.Title = updatedRequest.Title;
            course.Description = updatedRequest.Description;
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> PatchAsync(int id, CreateCourseRequest request)
        {
            // Samma som Update för nu
            return await UpdateAsync(id, request);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}