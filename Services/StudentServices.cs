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


        public List<Student> GetAllStudents()
        {
            return _studentRepository.GetAllStudents();
        }

        public Student? GetStudentById(int id)
        {
            return _studentRepository.GetStudentById(id);
        }

        public Student CreateStudent(CreateStudentRequest request)
        {
            return _studentRepository.CreateStudent(request);
        }

        public Student? UpdateStudent(int id, CreateStudentRequest updatedRequest)
        {
            return _studentRepository.UpdateStudent(id, updatedRequest);
        }

        public Student? PatchStudent(int id, Dictionary<string, string> updates)
        {
            return _studentRepository.PatchStudent(id, updates);
        }

        public bool DeleteStudent(int id)
        {
            return _studentRepository.DeleteStudent(id);
        }

    }
}