using System;
using System.Collections.Generic;
using Me.One.Core;

namespace MediatorTutorials.Core.Models
{
    public class Course : BaseEntity
    {
        public Course(Guid id)
        {
            Id = id.ToString();
        }

        public Course(string id)
        {
            Id = id;
        }

        public Course()
        {
        }

        public string Name { get; set; }
        public HashSet<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();
    }
}