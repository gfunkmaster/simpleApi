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
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentsController(StudentServices studentService, UserManager<ApplicationUser> userManager)
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
/// <summary>
    /// Hämtar en specifik student baserat på ID
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <returns>Student objekt</returns>
    /// <response code="200">Returnerar studenten</response>
    /// <response code="404">Student hittades inte</response>
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Hämtar inloggad användares profil med claims och kopplad student
        /// </summary>
        /// <returns>Användarinformation och claims</returns>
        /// <response code="200">Returnerar användarens profil</response>
        /// <response code="401">Användaren är inte autentiserad</response>
        /// <response code="404">Användaren hittades inte</response>
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Skapar en ny student
        /// </summary>
        /// <param name="request">Studentinformation</param>
        /// <returns>Den skapade studenten</returns>
        /// <response code="201">Student skapad</response>
        /// <response code="400">Ogiltig data</response>
        /// <response code="401">Användaren är inte autentiserad</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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