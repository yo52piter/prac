namespace prac.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
