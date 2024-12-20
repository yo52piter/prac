namespace prac.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
        public Course Course { get; set; }
    }
}
