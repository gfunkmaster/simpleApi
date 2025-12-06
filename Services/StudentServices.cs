using System.Linq;
using SimpleApi.src.Models;
using Repositories.Interfaces;

namespace Services
{
    public class StudentServices
    {
        private readonly IStudentRepository _studentRepository;

        public StudentServices(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllStudentsAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _studentRepository.GetStudentByIdAsync(id);
        }

        public async Task<Student> CreateStudentAsync(CreateStudentRequest request)
        {
            return await _studentRepository.CreateStudentAsync(request);
        }

        public async Task<Student?> UpdateStudentAsync(int id, CreateStudentRequest updatedRequest)
        {
            return await _studentRepository.UpdateStudentAsync(id, updatedRequest);
        }

        public async Task<Student?> PatchStudentAsync(int id, Dictionary<string, string> updates)
        {
            return await _studentRepository.PatchStudentAsync(id, updates);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await _studentRepository.DeleteStudentAsync(id);
        }
    }
}