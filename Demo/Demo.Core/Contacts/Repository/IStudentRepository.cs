using System.Collections.Generic;
using Demo.Core.CQRS.Queries;
using Demo.Core.Models;
using Me.One.Core.Contract.Repository;

namespace Demo.Core.Contacts.Repository
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        List<Student> GetStudentByCourse(GetStudentByCourse query);
        Student GetStudentById(string id);
        List<Student> GetListStudents();
    }
}