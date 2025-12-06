using SimpleApi.src.Models;
using SimpleApi.Data;
using Repositories.Interfaces;

namespace SimpleApi.Repositories
{
    public class EFStudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public EFStudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Student> GetAllStudents()
        {
            return _context.Students.ToList();
        }

        public Student? GetStudentById(int id)
        {
            return _context.Students.Find(id);
        }

        public Student CreateStudent(CreateStudentRequest request)
        {
            var student = new Student
            {
                Name = request.Name,
                Email = request.Email
            };
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

        public Student? UpdateStudent(int id, CreateStudentRequest updatedRequest)
        {
            var student = _context.Students.Find(id);
            if (student == null) return null;

            student.Name = updatedRequest.Name;
            student.Email = updatedRequest.Email;
            _context.SaveChanges();
            return student;
        }

        public Student? PatchStudent(int id, Dictionary<string, string> updates)
        {
            var student = _context.Students.Find(id);
            if (student == null) return null;

            foreach (var update in updates)
            {
                if (update.Key == "Name")
                    student.Name = update.Value;
                else if (update.Key == "Email")
                    student.Email = update.Value;
            }
            _context.SaveChanges();
            return student;
        }

        public bool DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            _context.SaveChanges();
            return true;
        }
    }
}