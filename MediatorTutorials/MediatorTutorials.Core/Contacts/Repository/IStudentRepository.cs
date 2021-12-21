using System.Collections.Generic;
using Me.One.Core.Contract.Repository;
using MediatorTutorials.Core.CQRS.Queries;
using MediatorTutorials.Core.Models;

namespace MediatorTutorials.Core.Contacts.Repository
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        List<Student> GetStudentByCourse(GetStudentByCourse query);
        Student GetStudentById(string id);
        List<Student> GetListStudents();
    }
}