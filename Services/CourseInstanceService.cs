using System.Collections.Generic;
using System.Linq;
using SimpleApi.src.Models;
using Repositories.Interfaces;

namespace Services
{
    public class CourseInstanceService
    {
        private readonly ICourseInstanceRepository _courseInstanceRepository;

        public CourseInstanceService(ICourseInstanceRepository courseInstanceRepository)
        {
            _courseInstanceRepository = courseInstanceRepository;
        }

        public List<CourseInstance> GetAll()
        {
            return _courseInstanceRepository.GetAll();
        }

        public CourseInstance? GetById(int id)
        {
            return _courseInstanceRepository.GetById(id);
        }

        public CourseInstance Add(CreateCourseInstanceRequest request)
        {
            return _courseInstanceRepository.Add(request);
        }

        public CourseInstance? Update(int id, CreateCourseInstanceRequest updatedRequest)
        {
            return _courseInstanceRepository.Update(id, updatedRequest);
        }

        public CourseInstance? Patch(int id, CreateCourseInstanceRequest patchRequest)
        {
            return _courseInstanceRepository.Patch(id, patchRequest);
        }

        public bool Delete(int id)
        {
            return _courseInstanceRepository.Delete(id);
        }

        public bool EnrollStudent(int courseInstanceId, int studentId)
        {
            return _courseInstanceRepository.EnrollStudent(courseInstanceId, studentId);
        }

        public bool UnenrollStudent(int courseInstanceId, int studentId)
        {
            return _courseInstanceRepository.UnenrollStudent(courseInstanceId, studentId);
        }
    }
}