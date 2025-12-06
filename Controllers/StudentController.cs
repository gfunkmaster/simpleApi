namespace SimpleApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SimpleApi.src.Models;
    using System.Collections.Generic;
    using Services;
    using System;

    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController: ControllerBase
    {

        private StudentServices _studentService;

        public StudentsController(StudentServices studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public ActionResult<List<Student>> GetAllStudents()
        {
            Console.WriteLine("GetAllStudents called!"); // Debug
            return Ok(_studentService.GetAllStudents());
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = _studentService.GetStudentById(id);
            if(student != null)
            {
                return Ok(student);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<Student> CreateStudent([FromBody] CreateStudentRequest request)
        {
            if(!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var newStudent = _studentService.CreateStudent(request);
            return CreatedAtAction(nameof(GetStudent), new { id = newStudent.Id }, newStudent);
        }

        [HttpPut("{id}")]
        public ActionResult<Student> UpdateStudent(int id, [FromBody] CreateStudentRequest updatedRequest)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var student = _studentService.UpdateStudent(id, updatedRequest);
            if (student == null) 
            {
                return NotFound();
            }
            
            return Ok(student);
        }

        [HttpPatch("{id}")]
        public ActionResult<Student> PatchStudent(int id, [FromBody] CreateStudentRequest updatedRequest)
        {
            var updates = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(updatedRequest.Name))
                updates["Name"] = updatedRequest.Name;
            if (!string.IsNullOrEmpty(updatedRequest.Email))
                updates["Email"] = updatedRequest.Email;
            var student = _studentService.PatchStudent(id, updates);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(int id)
        {
            var deleted = _studentService.DeleteStudent(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}