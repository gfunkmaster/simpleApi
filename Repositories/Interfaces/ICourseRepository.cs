using SimpleApi.src.Models;

namespace Repositories.Interfaces
{
    public interface ICourseRepository
    {
        List<Course> GetAll();
        Course? GetById(int id);
        Course Add(CreateCourseRequest request);
        Course? Update(int id, CreateCourseRequest request);
        Course? Patch(int id, CreateCourseRequest request);
        bool Delete(int id);
    }
}