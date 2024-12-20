namespace prac.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string ContentUrl { get; set; }
        public string Schedule { get; set; }

        public Course Course { get; set; }
    }
}
