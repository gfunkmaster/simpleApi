using SimpleApi.src.Models;

namespace Repositories.Interfaces
{
    public interface ICourseInstanceRepository
    {
        Task<List<CourseInstance>> GetAllAsync();
        Task<CourseInstance?> GetByIdAsync(int id);
        Task<CourseInstance> AddAsync(CreateCourseInstanceRequest request);
        Task<CourseInstance?> UpdateAsync(int id, CreateCourseInstanceRequest updatedRequest);
        Task<CourseInstance?> PatchAsync(int id, CreateCourseInstanceRequest patchRequest);
        Task<bool> DeleteAsync(int id);
        Task<bool> EnrollStudentAsync(int courseInstanceId, int studentId);
        Task<bool> UnenrollStudentAsync(int courseInstanceId, int studentId);
    }
}