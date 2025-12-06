
using SimpleApi.src.Models;

namespace Repositories.Interfaces
{
    public interface IStudentRepository
{
    List<Student> GetAllStudents();
    Student? GetStudentById(int id);
    Student CreateStudent(CreateStudentRequest request);
    Student? UpdateStudent(int id, CreateStudentRequest updatedRequest);
    Student? PatchStudent(int id, Dictionary<string, string> updates);
    bool DeleteStudent(int id);
}
}