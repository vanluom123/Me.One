using System.Collections.Generic;
using System.Linq;
using Demo.Core.Contacts.Repository;
using Demo.Core.CQRS.Queries;
using Demo.Core.Models;
using Me.One.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(DbContext context) : base(context)
        {
        }

        public List<Course> GetCourseByStudent(GetCourseByStudent query)
        {
            var courses = List(c => c.StudentCourses.Any(sc => sc.StudentId == query.Id)).ToList();
            return courses;
        }

        public Course GetCourseById(string id)
        {
            return this
                .Include(c => c.StudentCourses)
                .ThenInclude(sc => sc.Student)
                .FirstOrDefault(c => c.Id == id);
        }
    }
}