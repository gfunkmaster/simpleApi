
using SimpleApi.src.Models;

namespace Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(CreateStudentRequest request);
        Task<Student?> UpdateStudentAsync(int id, CreateStudentRequest updatedRequest);
        Task<Student?> PatchStudentAsync(int id, Dictionary<string, string> updates);
        Task<bool> DeleteStudentAsync(int id);
    }
}