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
        public ActionResult<List<Grade>> GetAllGrades()
        {
            return Ok(_gradeService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Grade> GetGrade(int id)
        {
            var grade = _gradeService.GetById(id);
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
        public ActionResult<Grade> CreateGrade([FromBody] CreateGradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var newGrade = _gradeService.Add(request);
                return CreatedAtAction(nameof(GetGrade), new { id = newGrade.Id }, newGrade);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Student or CourseInstance not found
            }
        }

        [HttpGet("student/{studentId}")]
        public ActionResult<List<Grade>> GetGradesByStudent(int studentId)
        {
            var grades = _gradeService.GetGradesByStudent(studentId);
            return Ok(grades);
        }

        [HttpGet("courseinstance/{courseInstanceId}")]
        public ActionResult<List<Grade>> GetGradesByCourseInstance(int courseInstanceId)
        {
            var grades = _gradeService.GetGradesByCourseInstance(courseInstanceId);
            return Ok(grades);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteGrade(int id)
        {
            var deleted = _gradeService.Delete(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}