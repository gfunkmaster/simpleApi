using SimpleApi.src.Models;

namespace Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<Course> AddAsync(CreateCourseRequest request);
        Task<Course?> UpdateAsync(int id, CreateCourseRequest request);
        Task<Course?> PatchAsync(int id, CreateCourseRequest request);
        Task<bool> DeleteAsync(int id);
    }
}