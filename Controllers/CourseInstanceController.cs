
using Microsoft.AspNetCore.Mvc;
using SimpleApi.src.Models;
using Services;

namespace SimpleApi.Controllers
{
    [ApiController]
    [Route("api/CourseInstances")]
    public class CourseInstancesController : ControllerBase
    {
        private readonly CourseInstanceService _courseInstanceService;

        public CourseInstancesController(CourseInstanceService courseInstanceService)
        {
            _courseInstanceService = courseInstanceService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseInstance>>> GetAllCourseInstances()
        {
            var instances = await _courseInstanceService.GetAllAsync();
            return Ok(instances);
        }

        [HttpGet("{id}")]
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

        [HttpPost]
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

        [HttpPost("{courseInstanceId}/enroll/{studentId}")]
        public async Task<ActionResult> EnrollStudent(int courseInstanceId, int studentId)
        {
            var success = await _courseInstanceService.EnrollStudentAsync(courseInstanceId, studentId);
            if (success)
            {
                return Ok(new { message = $"Student {studentId} enrolled in course instance {courseInstanceId}" });
            }
            return BadRequest(new { message = "Failed to enroll student. Course instance or student not found, or student already enrolled." });
        }

        [HttpDelete("{courseInstanceId}/unenroll/{studentId}")]
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