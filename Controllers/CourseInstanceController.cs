
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
        public ActionResult<List<CourseInstance>> GetAllCourseInstances()
        {
            return Ok(_courseInstanceService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CourseInstance> GetCourseInstance(int id)
        {
            var courseInstance = _courseInstanceService.GetById(id);
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
        public ActionResult<CourseInstance> CreateCourseInstance([FromBody] CreateCourseInstanceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            
            try
            {
                var newCourseInstance = _courseInstanceService.Add(request);
                return CreatedAtAction(nameof(GetCourseInstance), new { id = newCourseInstance.Id }, newCourseInstance);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Course not found -> 404
            }
        }

        [HttpPut("{id}")]
        public ActionResult<CourseInstance> UpdateCourseInstance(int id, [FromBody] CreateCourseInstanceRequest updatedRequest)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            
            try
            {
                var updated = _courseInstanceService.Update(id, updatedRequest);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public ActionResult<CourseInstance> PatchCourseInstance(int id, [FromBody] CreateCourseInstanceRequest patchRequest)
        {
            try
            {
                var patched = _courseInstanceService.Patch(id, patchRequest);
                if (patched == null) return NotFound();
                return Ok(patched);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCourseInstance(int id)
        {
            var deleted = _courseInstanceService.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // Extra endpoints från tidigare uppgifter
        [HttpGet("date-range/{fromDate}/{toDate}")]
        public ActionResult<IEnumerable<CourseInstance>> GetCourseInstancesByDateRange(DateTime fromDate, DateTime toDate)
        {
            var filteredInstances = _courseInstanceService.GetAll().Where(ci => ci.StartDate >= fromDate && ci.EndDate <= toDate);
            return Ok(filteredInstances);
        }

        [HttpGet("student/{studentId}")]
        public ActionResult<IEnumerable<CourseInstance>> GetCourseInstancesByStudent(int studentId)
        {
            var filteredInstances = _courseInstanceService.GetAll().Where(ci => ci.EnrolledStudents.Any(s => s.Id == studentId));
            return Ok(filteredInstances);
        }

        [HttpPost("{courseInstanceId}/enroll/{studentId}")]
        public ActionResult EnrollStudent(int courseInstanceId, int studentId)
        {
            var success = _courseInstanceService.EnrollStudent(courseInstanceId, studentId);
            if (success)
            {
                return Ok(new { message = $"Student {studentId} enrolled in course instance {courseInstanceId}" });
            }
            return BadRequest(new { message = "Failed to enroll student. Course instance or student not found, or student already enrolled." });
        }

        [HttpDelete("{courseInstanceId}/unenroll/{studentId}")]
        public ActionResult UnenrollStudent(int courseInstanceId, int studentId)
        {
            var success = _courseInstanceService.UnenrollStudent(courseInstanceId, studentId);
            if (success)
            {
                return Ok(new { message = $"Student {studentId} unenrolled from course instance {courseInstanceId}" });
            }
            return BadRequest(new { message = "Failed to unenroll student. Course instance or student not found." });
        }
    }
}