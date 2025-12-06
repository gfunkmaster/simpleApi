using SimpleApi.src.Models;

namespace Repositories.Interfaces
{
    public interface IGradeRepository
    {
        Task<List<Grade>> GetAllAsync();
        Task<Grade?> GetByIdAsync(int id);
        Task<Grade> AddAsync(CreateGradeRequest request);
        Task<bool> DeleteAsync(int id);
        Task<List<Grade>> GetGradesByStudentAsync(int studentId);
        Task<List<Grade>> GetGradesByCourseInstanceAsync(int courseInstanceId);
    }
}