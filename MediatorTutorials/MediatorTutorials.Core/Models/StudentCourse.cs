namespace MediatorTutorials.Core.Models
{
    public class StudentCourse
    {
        public string CourseId { get; set; }
        public Course Course { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; }
    }
}