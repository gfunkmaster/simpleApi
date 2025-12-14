namespace SimpleApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SimpleApi.src.Models;
    using Services;

    /// <summary>
    /// Hanterar kurs-relaterade operationer
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CoursesController(CourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Hämtar alla kurser
        /// </summary>
        /// <returns>Lista med alla kurser</returns>
        /// <response code="200">Returnerar lista med kurser</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Course>>> GetAllCourses()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(courses);
        }

        /// <summary>
        /// Hämtar en specifik kurs baserat på ID
        /// </summary>
        /// <param name="id">Kurs ID</param>
        /// <returns>Kurs objekt</returns>
        /// <response code="200">Returnerar kursen</response>
        /// <response code="404">Kursen hittades inte</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Course>> GetCourseById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if(course != null)
            {
                return Ok(course);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Skapar en ny kurs
        /// </summary>
        /// <param name="request">Kursinformation</param>
        /// <returns>Den skapade kursen</returns>
        /// <response code="201">Kurs skapad</response>
        /// <response code="400">Ogiltig data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] CreateCourseRequest request)
        {
            if(!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var newCourse = await _courseService.AddAsync(request);
            return CreatedAtAction(nameof(GetCourseById), new { id = newCourse.Id }, newCourse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Course>> UpdateCourse(int id, [FromBody] CreateCourseRequest request)
        {
            if(string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Description))
            {
                return BadRequest("Title and Description are required");
            }
            var updated = await _courseService.UpdateAsync(id, request);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Course>> PatchCourse(int id, [FromBody] CreateCourseRequest request)
        {
            var patched = await _courseService.PatchAsync(id, request);
            if (patched == null) return NotFound();
            return Ok(patched);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var deleted = await _courseService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}