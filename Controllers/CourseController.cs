using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prac.Data;
using prac.DTOs;
using prac.Models;

namespace prac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    InstructorId = c.InstructorId,
                    InstructorName = c.Instructor.Name
                })
                .ToListAsync();

            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.CourseId == id)
                .Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    InstructorId = c.InstructorId,
                    InstructorName = c.Instructor.Name
                })
                .FirstOrDefaultAsync();

            if (course == null) return NotFound();

            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseDto courseDto)
        {
            var course = new Course
            {
                Title = courseDto.Title,
                Description = courseDto.Description,
                InstructorId = courseDto.InstructorId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            // Возвращаем DTO нового курса
            var createdCourseDto = new CourseDto
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                InstructorId = course.InstructorId
            };

            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, createdCourseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, CourseDto courseDto)
        {
            if (id != courseDto.CourseId) return BadRequest();

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            course.Title = courseDto.Title;
            course.Description = courseDto.Description;
            course.InstructorId = courseDto.InstructorId;

            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
