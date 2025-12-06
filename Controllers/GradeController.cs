using Microsoft.AspNetCore.Mvc;
using SimpleApi.src.Models;
using Services;

namespace SimpleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly GradeService _gradeService;

        public GradesController(GradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Grade>>> GetAllGrades()
        {
            var grades = await _gradeService.GetAllAsync();
            return Ok(grades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> GetGrade(int id)
        {
            var grade = await _gradeService.GetByIdAsync(id);
            if (grade != null)
            {
                return Ok(grade);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Grade>> CreateGrade([FromBody] CreateGradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var newGrade = await _gradeService.AddAsync(request);
                return CreatedAtAction(nameof(GetGrade), new { id = newGrade.Id }, newGrade);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Student or CourseInstance not found
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<Grade>>> GetGradesByStudent(int studentId)
        {
            var grades = await _gradeService.GetGradesByStudentAsync(studentId);
            return Ok(grades);
        }

        [HttpGet("courseinstance/{courseInstanceId}")]
        public async Task<ActionResult<List<Grade>>> GetGradesByCourseInstance(int courseInstanceId)
        {
            var grades = await _gradeService.GetGradesByCourseInstanceAsync(courseInstanceId);
            return Ok(grades);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGrade(int id)
        {
            var deleted = await _gradeService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}