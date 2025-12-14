
using Microsoft.AspNetCore.Mvc;
using SimpleApi.src.Models;
using Services;

namespace SimpleApi.Controllers
{
    /// <summary>
    /// Hanterar kursinstans-relaterade operationer inklusive enrollment
    /// </summary>
    [ApiController]
    [Route("api/CourseInstances")]
    public class CourseInstancesController : ControllerBase
    {
        private readonly CourseInstanceService _courseInstanceService;

        public CourseInstancesController(CourseInstanceService courseInstanceService)
        {
            _courseInstanceService = courseInstanceService;
        }

        /// <summary>
        /// Hämtar alla kursinstanser
        /// </summary>
        /// <returns>Lista med alla kursinstanser</returns>
        /// <response code="200">Returnerar lista med kursinstanser</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CourseInstance>>> GetAllCourseInstances()
        {
            var instances = await _courseInstanceService.GetAllAsync();
            return Ok(instances);
        }

        /// <summary>
        /// Hämtar en specifik kursinstans baserat på ID
        /// </summary>
        /// <param name="id">Kursinstans ID</param>
        /// <returns>Kursinstans objekt med enrolled studenter</returns>
        /// <response code="200">Returnerar kursinstansen</response>
        /// <response code="404">Kursinstansen hittades inte</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseInstance>> GetCourseInstance(int id)
        {
            var courseInstance = await _courseInstanceService.GetByIdAsync(id);
            if(courseInstance != null)
            {
                return Ok(courseInstance);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Skapar en ny kursinstans
        /// </summary>
        /// <param name="request">Kursinstans information (startDate, endDate, courseId)</param>
        /// <returns>Den skapade kursinstansen</returns>
        /// <response code="201">Kursinstans skapad</response>
        /// <response code="400">Ogiltig data</response>
        /// <response code="404">Kursen hittades inte</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseInstance>> CreateCourseInstance([FromBody] CreateCourseInstanceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            try
            {
                var newCourseInstance = await _courseInstanceService.AddAsync(request);
                return CreatedAtAction(nameof(GetCourseInstance), new { id = newCourseInstance.Id }, newCourseInstance);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Course not found -> 404
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseInstance>> UpdateCourseInstance(int id, [FromBody] CreateCourseInstanceRequest updatedRequest)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            try
            {
                var updated = await _courseInstanceService.UpdateAsync(id, updatedRequest);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CourseInstance>> PatchCourseInstance(int id, [FromBody] CreateCourseInstanceRequest patchRequest)
        {
            try
            {
                var patched = await _courseInstanceService.PatchAsync(id, patchRequest);
                if (patched == null) return NotFound();
                return Ok(patched);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourseInstance(int id)
        {
            var deleted = await _courseInstanceService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // Extra endpoints från tidigare uppgifter
        [HttpGet("date-range/{fromDate}/{toDate}")]
        public async Task<ActionResult<IEnumerable<CourseInstance>>> GetCourseInstancesByDateRange(DateTime fromDate, DateTime toDate)
        {
            var allInstances = await _courseInstanceService.GetAllAsync();
            var filteredInstances = allInstances.Where(ci => ci.StartDate >= fromDate && ci.EndDate <= toDate);
            return Ok(filteredInstances);
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<CourseInstance>>> GetCourseInstancesByStudent(int studentId)
        {
            var allInstances = await _courseInstanceService.GetAllAsync();
            var filteredInstances = allInstances.Where(ci => ci.EnrolledStudents.Any(s => s.Id == studentId));
            return Ok(filteredInstances);
        }

        /// <summary>
        /// Registrerar en student till en kursinstans (Many-to-Many)
        /// </summary>
        /// <param name="courseInstanceId">Kursinstans ID</param>
        /// <param name="studentId">Student ID</param>
        /// <returns>Bekräftelse på enrollment</returns>
        /// <response code="200">Student registrerad</response>
        /// <response code="400">Fel vid enrollment (redan registrerad eller objekt saknas)</response>
        [HttpPost("{courseInstanceId}/enroll/{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EnrollStudent(int courseInstanceId, int studentId)
        {
            var success = await _courseInstanceService.EnrollStudentAsync(courseInstanceId, studentId);
            if (success)
            {
                return Ok(new { message = $"Student {studentId} enrolled in course instance {courseInstanceId}" });
            }
            return BadRequest(new { message = "Failed to enroll student. Course instance or student not found, or student already enrolled." });
        }

        /// <summary>
        /// Avregistrerar en student från en kursinstans
        /// </summary>
        /// <param name="courseInstanceId">Kursinstans ID</param>
        /// <param name="studentId">Student ID</param>
        /// <returns>Bekräftelse på unenrollment</returns>
        /// <response code="200">Student avregistrerad</response>
        /// <response code="400">Fel vid avregistrering (objekt saknas)</response>
        [HttpDelete("{courseInstanceId}/unenroll/{studentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UnenrollStudent(int courseInstanceId, int studentId)
        {
            var success = await _courseInstanceService.UnenrollStudentAsync(courseInstanceId, studentId);
            if (success)
            {
                return Ok(new { message = $"Student {studentId} unenrolled from course instance {courseInstanceId}" });
            }
            return BadRequest(new { message = "Failed to unenroll student. Course instance or student not found." });
        }
    }
}