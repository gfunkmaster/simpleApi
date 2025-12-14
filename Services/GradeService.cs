using SimpleApi.src.Models;
using Repositories.Interfaces;

namespace Services
{
    public class GradeService(IGradeRepository gradeRepository)
    {
        private readonly IGradeRepository _gradeRepository = gradeRepository;

        public async Task<List<Grade>> GetAllAsync()
        {
            return await _gradeRepository.GetAllAsync();
        }

        public async Task<Grade?> GetByIdAsync(int id)
        {
            return await _gradeRepository.GetByIdAsync(id);
        }

        public async Task<Grade> AddAsync(CreateGradeRequest request)
        {
            return await _gradeRepository.AddAsync(request);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _gradeRepository.DeleteAsync(id);
        }

        public async Task<List<Grade>> GetGradesByStudentAsync(int studentId)
        {
            return await _gradeRepository.GetGradesByStudentAsync(studentId);
        }

        public async Task<List<Grade>> GetGradesByCourseInstanceAsync(int courseInstanceId)
        {
            return await _gradeRepository.GetGradesByCourseInstanceAsync(courseInstanceId);
        }
    }
}