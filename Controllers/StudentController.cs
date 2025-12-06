namespace SimpleApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using SimpleApi.src.Models;
    using System.Collections.Generic;
    using Services;
    using System;

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class StudentsController: ControllerBase
    {

    private readonly StudentServices _studentService;
    private readonly UserManager<IdentityUser> _userManager;

    public StudentsController(StudentServices studentService, UserManager<IdentityUser> userManager)
    {
        _studentService = studentService;
        _userManager = userManager;
    }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            Console.WriteLine("GetAllStudents called!"); // Debug
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if(student != null)
            {
                return Ok(student);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("profile")]
        public async Task<ActionResult> GetProfile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type.EndsWith("nameidentifier"))?.Value;
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var claimedId = User.Claims.Select(c => new { Type = c.Type.Split('/').Last(), c.Value }).ToList();
            var students = await _studentService.GetAllStudentsAsync();
            var student = students.Find(s => s.Email == user.Email);

            return Ok(new {
                UserId = user.Id,
                Email = user.Email,
                ClaimedId = claimedId,
                Student = student
            });
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent([FromBody] CreateStudentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var newStudent = await _studentService.CreateStudentAsync(request);
            return CreatedAtAction(nameof(GetStudent), new { id = newStudent.Id }, newStudent);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(int id, [FromBody] CreateStudentRequest updatedRequest)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var student = await _studentService.UpdateStudentAsync(id, updatedRequest);
            if (student == null) 
            {
                return NotFound();
            }
            
            return Ok(student);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Student>> PatchStudent(int id, [FromBody] CreateStudentRequest updatedRequest)
        {
            var updates = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(updatedRequest.Name))
                updates["Name"] = updatedRequest.Name;
            if (!string.IsNullOrEmpty(updatedRequest.Email))
                updates["Email"] = updatedRequest.Email;
            var student = await _studentService.PatchStudentAsync(id, updates);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var deleted = await _studentService.DeleteStudentAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}