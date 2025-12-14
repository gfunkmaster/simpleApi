using System.Collections.Generic;
using System.Linq;
using SimpleApi.src.Models;
using Repositories.Interfaces;

namespace Services
{
    public class CourseService(ICourseRepository courseRepository)
    {
        private readonly ICourseRepository _courseRepository = courseRepository;

        public async Task<List<Course>> GetAllAsync()
        {
            return await _courseRepository.GetAllAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task<Course> AddAsync(CreateCourseRequest request)
        {
            return await _courseRepository.AddAsync(request);
        }

        public async Task<Course?> UpdateAsync(int id, CreateCourseRequest request)
        {
            return await _courseRepository.UpdateAsync(id, request);
        }

        public async Task<Course?> PatchAsync(int id, CreateCourseRequest request)
        {
            return await _courseRepository.PatchAsync(id, request);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _courseRepository.DeleteAsync(id);
        }
    }
}