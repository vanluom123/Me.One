using System;
using System.Collections.Generic;
using Me.One.Core;

namespace Demo.Core.Models
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
        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}