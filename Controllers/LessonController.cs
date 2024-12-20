using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prac.Data;
using prac.DTOs;
using prac.Models;

namespace prac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessons()
        {
            var lessons = await _context.Lessons
                .Include(l => l.Course)
                .Select(l => new LessonDto
                {
                    LessonId = l.LessonId,
                    CourseId = l.CourseId,
                    CourseTitle = l.Course.Title,
                    Title = l.Title,
                    ContentUrl = l.ContentUrl,
                    Schedule = l.Schedule
                })
                .ToListAsync();

            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetLesson(int id)
        {
            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .Where(l => l.LessonId == id)
                .Select(l => new LessonDto
                {
                    LessonId = l.LessonId,
                    CourseId = l.CourseId,
                    CourseTitle = l.Course.Title,
                    Title = l.Title,
                    ContentUrl = l.ContentUrl,
                    Schedule = l.Schedule
                })
                .FirstOrDefaultAsync();

            if (lesson == null) return NotFound();

            return Ok(lesson);
        }

        [HttpPost]
        public async Task<ActionResult<LessonDto>> CreateLesson(LessonDto lessonDto)
        {
            var lesson = new Lesson
            {
                CourseId = lessonDto.CourseId,
                Title = lessonDto.Title,
                ContentUrl = lessonDto.ContentUrl,
                Schedule = lessonDto.Schedule
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            lessonDto.LessonId = lesson.LessonId;

            return CreatedAtAction(nameof(GetLesson), new { id = lesson.LessonId }, lessonDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, LessonDto lessonDto)
        {
            if (id != lessonDto.LessonId) return BadRequest();

            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return NotFound();

            lesson.CourseId = lessonDto.CourseId;
            lesson.Title = lessonDto.Title;
            lesson.ContentUrl = lessonDto.ContentUrl;
            lesson.Schedule = lessonDto.Schedule;

            _context.Entry(lesson).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return NotFound();

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
