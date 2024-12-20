using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prac.Data;
using prac.DTOs;
using prac.Models;

namespace prac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentDto>>> GetEnrollments()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    UserId = e.UserId,
                    UserName = e.User.Name,
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.Title,
                    Status = e.Status
                })
                .ToListAsync();

            return Ok(enrollments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDto>> GetEnrollment(int id)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .Where(e => e.EnrollmentId == id)
                .Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    UserId = e.UserId,
                    UserName = e.User.Name,
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.Title,
                    Status = e.Status
                })
                .FirstOrDefaultAsync();

            if (enrollment == null) return NotFound();

            return Ok(enrollment);
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentDto>> CreateEnrollment(EnrollmentDto enrollmentDto)
        {
            var enrollment = new Enrollment
            {
                UserId = enrollmentDto.UserId,
                CourseId = enrollmentDto.CourseId,
                Status = enrollmentDto.Status
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            enrollmentDto.EnrollmentId = enrollment.EnrollmentId;

            return CreatedAtAction(nameof(GetEnrollment), new { id = enrollment.EnrollmentId }, enrollmentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnrollment(int id, EnrollmentDto enrollmentDto)
        {
            if (id != enrollmentDto.EnrollmentId) return BadRequest();

            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            enrollment.UserId = enrollmentDto.UserId;
            enrollment.CourseId = enrollmentDto.CourseId;
            enrollment.Status = enrollmentDto.Status;

            _context.Entry(enrollment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
