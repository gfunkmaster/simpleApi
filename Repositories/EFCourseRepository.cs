using SimpleApi.src.Models;
using SimpleApi.Data;
using Repositories.Interfaces;

namespace SimpleApi.Repositories
{
    public class EFCourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public Course? GetById(int id)
        {
            return _context.Courses.Find(id);
        }

        public Course Add(CreateCourseRequest request)
        {
            var course = new Course
            {
                Title = request.Title,
                Description = request.Description
            };
            _context.Courses.Add(course);
            _context.SaveChanges();
            return course;
        }

        public Course? Update(int id, CreateCourseRequest updatedRequest)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return null;

            course.Title = updatedRequest.Title;
            course.Description = updatedRequest.Description;
            _context.SaveChanges();
            return course;
        }

        public Course? Patch(int id, CreateCourseRequest request)
        {
            // Samma som Update för nu
            return Update(id, request);
        }

        public bool Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            _context.SaveChanges();
            return true;
        }
    }
}