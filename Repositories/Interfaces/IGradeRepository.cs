using SimpleApi.src.Models;

namespace Repositories.Interfaces
{
    public interface IGradeRepository
    {
        List<Grade> GetAll();
        Grade? GetById(int id);
        Grade Add(CreateGradeRequest request);
        bool Delete(int id);
        List<Grade> GetGradesByStudent(int studentId);
        List<Grade> GetGradesByCourseInstance(int courseInstanceId);
    }
}