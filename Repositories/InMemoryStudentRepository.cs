
using SimpleApi.src.Models;
using Repositories.Interfaces;

namespace Repositories
{
    public class InMemoryStudentRepository : IStudentRepository
{
    private readonly List<Student> _students = 
        [
                new (1, "Alice Johnson", "alice.johnson@example.com"),
                new (2, "Bob Smith", "bob.smith@example.com"),
                new (3, "Charlie Brown", "charlie.brown@example.com")
        ];

    public List<Student> GetAllStudents()
    {
        return _students;
    }

    public Student? GetStudentById(int id)
    {
        return _students.FirstOrDefault(s => s.Id == id);
    }

    public Student CreateStudent(CreateStudentRequest request)
    {
        int newId = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
        var newStudent = new Student(newId, request.Name, request.Email);
        _students.Add(newStudent);
        return newStudent;
    }

    public Student? UpdateStudent(int id, CreateStudentRequest updatedRequest)
    {
        var student = GetStudentById(id);
        if (student == null) return null;

        student.Name = updatedRequest.Name;
        student.Email = updatedRequest.Email;
        return student;
    }

    public Student? PatchStudent(int id, Dictionary<string, string> updates)
    {
        var student = GetStudentById(id);
        if (student == null) return null;

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
        return student;
    }

    public bool DeleteStudent(int id)
    {
        var student = GetStudentById(id);
        if (student == null) return false;

        _students.Remove(student);
        return true;
    }
}
}