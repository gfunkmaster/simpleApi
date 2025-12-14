using Microsoft.AspNetCore.Mvc;
using SimpleApi.src.Models;
using Services;

namespace SimpleApi.Controllers
{
    /// <summary>
    /// Hanterar betyg för studenter i kursinstanser
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly GradeService _gradeService;

        public GradesController(GradeService gradeService)
        {
            _gradeService = gradeService;
        }

        /// <summary>
        /// Hämtar alla betyg med nested student och course information
        /// </summary>
        /// <returns>Lista med alla betyg</returns>
        /// <response code="200">Returnerar lista med betyg</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Grade>>> GetAllGrades()
        {
            var grades = await _gradeService.GetAllAsync();
            return Ok(grades);
        }

        /// <summary>
        /// Hämtar ett specifikt betyg baserat på ID
        /// </summary>
        /// <param name="id">Betyg ID</param>
        /// <returns>Betyg objekt</returns>
        /// <response code="200">Returnerar betyget</response>
        /// <response code="404">Betyget hittades inte</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Skapar ett nytt betyg för en student i en kursinstans
        /// </summary>
        /// <param name="request">Betyg information (value, studentId, courseInstanceId)</param>
        /// <returns>Det skapade betyget</returns>
        /// <response code="201">Betyg skapat</response>
        /// <response code="400">Ogiltig data</response>
        /// <response code="404">Student eller kursinstans hittades inte</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Hämtar alla betyg för en specifik student
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <returns>Lista med studentens betyg</returns>
        /// <response code="200">Returnerar studentens betyg</response>
        [HttpGet("student/{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Grade>>> GetGradesByStudent(int studentId)
        {
            var grades = await _gradeService.GetGradesByStudentAsync(studentId);
            return Ok(grades);
        }

        /// <summary>
        /// Hämtar alla betyg för en specifik kursinstans
        /// </summary>
        /// <param name="courseInstanceId">Kursinstans ID</param>
        /// <returns>Lista med betyg för kursinstansen</returns>
        /// <response code="200">Returnerar kursinstansens betyg</response>
        [HttpGet("courseinstance/{courseInstanceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Grade>>> GetGradesByCourseInstance(int courseInstanceId)
        {
            var grades = await _gradeService.GetGradesByCourseInstanceAsync(courseInstanceId);
            return Ok(grades);
        }

        /// <summary>
        /// Tar bort ett betyg
        /// </summary>
        /// <param name="id">Betyg ID</param>
        /// <returns>No content vid framgång</returns>
        /// <response code="204">Betyg borttaget</response>
        /// <response code="404">Betyget hittades inte</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteGrade(int id)
        {
            var deleted = await _gradeService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}