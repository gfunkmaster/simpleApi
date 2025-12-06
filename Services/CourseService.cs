using System.Collections.Generic;
using System.Linq;
using SimpleApi.src.Models;
using Repositories.Interfaces;

namespace Services
{
    public class CourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public List<Course> GetAll()
        {
            return _courseRepository.GetAll();
        }

        public Course? GetById(int id)
        {
            return _courseRepository.GetById(id);
        }

        public Course Add(CreateCourseRequest request)
        {
            return _courseRepository.Add(request);
        }

        public Course? Update(int id, CreateCourseRequest request)
        {
            return _courseRepository.Update(id, request);
        }

        public Course? Patch(int id, CreateCourseRequest request)
        {
            return _courseRepository.Patch(id, request);
        }

        public bool Delete(int id)
        {
            return _courseRepository.Delete(id);
        }
    }
}