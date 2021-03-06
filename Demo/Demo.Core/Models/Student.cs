using System;
using System.Collections.Generic;
using Me.One.Core;

namespace Demo.Core.Models
{
    public class Student : BaseEntity
    {
        public Student(Guid id)
        {
            Id = id.ToString();
        }

        public Student(string id)
        {
            Id = id;
        }

        public Student()
        {
        }

        public string FullName { get; set; }
        public string DoB { get; set; }
        public string Gender { get; set; }
        public string Classmate { get; set; }
        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}