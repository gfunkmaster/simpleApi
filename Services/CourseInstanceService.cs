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

        public async Task<List<CourseInstance>> GetAllAsync()
        {
            return await _courseInstanceRepository.GetAllAsync();
        }

        public async Task<CourseInstance?> GetByIdAsync(int id)
        {
            return await _courseInstanceRepository.GetByIdAsync(id);
        }

        public async Task<CourseInstance> AddAsync(CreateCourseInstanceRequest request)
        {
            return await _courseInstanceRepository.AddAsync(request);
        }

        public async Task<CourseInstance?> UpdateAsync(int id, CreateCourseInstanceRequest updatedRequest)
        {
            return await _courseInstanceRepository.UpdateAsync(id, updatedRequest);
        }

        public async Task<CourseInstance?> PatchAsync(int id, CreateCourseInstanceRequest patchRequest)
        {
            return await _courseInstanceRepository.PatchAsync(id, patchRequest);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _courseInstanceRepository.DeleteAsync(id);
        }

        public async Task<bool> EnrollStudentAsync(int courseInstanceId, int studentId)
        {
            return await _courseInstanceRepository.EnrollStudentAsync(courseInstanceId, studentId);
        }

        public async Task<bool> UnenrollStudentAsync(int courseInstanceId, int studentId)
        {
            return await _courseInstanceRepository.UnenrollStudentAsync(courseInstanceId, studentId);
        }
    }
}