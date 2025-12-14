using SimpleApi.src.Models;
using SimpleApi.Data;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SimpleApi.Repositories
{
    public class EFStudentRepository(ApplicationDbContext context) : IStudentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> CreateStudentAsync(CreateStudentRequest request)
        {
            var student = new Student
            {
                Name = request.Name,
                Email = request.Email
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> UpdateStudentAsync(int id, CreateStudentRequest updatedRequest)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return null;

            student.Name = updatedRequest.Name;
            student.Email = updatedRequest.Email;
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> PatchStudentAsync(int id, Dictionary<string, string> updates)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return null;

            foreach (var update in updates)
            {
                if (update.Key == "Name")
                    student.Name = update.Value;
                else if (update.Key == "Email")
                    student.Email = update.Value;
            }
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}