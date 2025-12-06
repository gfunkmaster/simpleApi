namespace SimpleApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SimpleApi.src.Models;
    using Services;

    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CoursesController(CourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public ActionResult<List<Course>> GetAllCourses()
        {
            return Ok(_courseService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Course> GetCourseById(int id)
        {
            var course = _courseService.GetAll().FirstOrDefault(c => c.Id == id);
            if(course != null)
            {
                return Ok(course);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<Course> CreateCourse([FromBody] CreateCourseRequest request)
        {
            if(!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var newCourse = _courseService.Add(request);
            return CreatedAtAction(nameof(GetCourseById), new { id = newCourse.Id }, newCourse);
        }

        [HttpPut("{id}")]
        public ActionResult<Course> UpdateCourse(int id, [FromBody] CreateCourseRequest request)
        {
            if(string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Description))
            {
                return BadRequest("Title and Description are required");
            }
            var updated = _courseService.Update(id, request);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpPatch("{id}")]
        public ActionResult<Course> PatchCourse(int id, [FromBody] CreateCourseRequest request)
        {
            var patched = _courseService.Patch(id, request);
            if (patched == null) return NotFound();
            return Ok(patched);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCourse(int id)
        {
            var deleted = _courseService.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}