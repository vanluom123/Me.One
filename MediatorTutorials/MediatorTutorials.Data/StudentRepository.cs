using System.Collections.Generic;
using System.Linq;
using Me.One.Core.Data;
using MediatorTutorials.Core.Contacts.Repository;
using MediatorTutorials.Core.CQRS.Queries;
using MediatorTutorials.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MediatorTutorials.Data
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
    }
}