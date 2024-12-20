namespace prac.DTOs
{
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string Status { get; set; }
    }
}
