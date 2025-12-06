
using SimpleApi.src.Models;
using Repositories.Interfaces;

namespace Repositories
{
    public class InMemoryStudentRepository : IStudentRepository
    {
        private readonly List<Student> _students =
        [
            new(1, "Alice Johnson", "alice.johnson@example.com"),
            new(2, "Bob Smith", "bob.smith@example.com"),
            new(3, "Charlie Brown", "charlie.brown@example.com")
        ];

        public Task<List<Student>> GetAllStudentsAsync()
        {
            return Task.FromResult(_students.ToList());
        }

        public Task<Student?> GetStudentByIdAsync(int id)
        {
            return Task.FromResult(_students.FirstOrDefault(s => s.Id == id));
        }

        public Task<Student?> CreateStudentAsync(CreateStudentRequest request)
        {
            int newId = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
            var newStudent = new Student(newId, request.Name, request.Email);
            _students.Add(newStudent);
            return Task.FromResult<Student?>(newStudent);
        }

        public Task<Student?> UpdateStudentAsync(int id, CreateStudentRequest updatedRequest)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return Task.FromResult<Student?>(null);

            student.Name = updatedRequest.Name;
            student.Email = updatedRequest.Email;
            return Task.FromResult(student);
        }

        public Task<Student?> PatchStudentAsync(int id, Dictionary<string, string> updates)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return Task.FromResult<Student?>(null);

            foreach (var update in updates)
            {
                if (update.Key == "Name")
                {
                    student.Name = update.Value;
                }
                else if (update.Key == "Email")
                {
                    student.Email = update.Value;
                }
            }
            return Task.FromResult(student);
        }

        public Task<bool> DeleteStudentAsync(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null) return Task.FromResult(false);

            _students.Remove(student);
            return Task.FromResult(true);
        }
    }
}