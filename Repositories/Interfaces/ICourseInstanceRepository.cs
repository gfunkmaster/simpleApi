using SimpleApi.src.Models;

namespace Repositories.Interfaces
{
    public interface ICourseInstanceRepository
    {
        List<CourseInstance> GetAll();
        CourseInstance? GetById(int id);
        CourseInstance Add(CreateCourseInstanceRequest request);
        CourseInstance? Update(int id, CreateCourseInstanceRequest updatedRequest);
        CourseInstance? Patch(int id, CreateCourseInstanceRequest patchRequest);
        bool Delete(int id);
        bool EnrollStudent(int courseInstanceId, int studentId);
        bool UnenrollStudent(int courseInstanceId, int studentId);
    }
}