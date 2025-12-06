using SimpleApi.src.Models;
using Repositories.Interfaces;

namespace Services
{
    public class GradeService
    {
        private readonly IGradeRepository _gradeRepository;

        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        public List<Grade> GetAll()
        {
            return _gradeRepository.GetAll();
        }

        public Grade? GetById(int id)
        {
            return _gradeRepository.GetById(id);
        }

        public Grade Add(CreateGradeRequest request)
        {
            return _gradeRepository.Add(request);
        }

        public bool Delete(int id)
        {
            return _gradeRepository.Delete(id);
        }

        public List<Grade> GetGradesByStudent(int studentId)
        {
            return _gradeRepository.GetGradesByStudent(studentId);
        }

        public List<Grade> GetGradesByCourseInstance(int courseInstanceId)
        {
            return _gradeRepository.GetGradesByCourseInstance(courseInstanceId);
        }
    }
}