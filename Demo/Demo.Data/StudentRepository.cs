using System.Collections.Generic;
using System.Linq;
using Demo.Core.Contacts.Repository;
using Demo.Core.CQRS.Queries;
using Demo.Core.Models;
using Me.One.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(DbContext context)
            : base(context)
        {
        }

        public List<Student> GetStudentByCourse(GetStudentByCourse query)
        {
            var students = List(s => s.StudentCourses.Any(sc => sc.CourseId == query.Id)).ToList();
            return students;
        }

        public Student GetStudentById(string id)
        {
            return this
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefault(s => s.Id == id);
        }

        public List<Student> GetListStudents()
        {
            return this
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .List()
                .ToList();
        }
    }
}