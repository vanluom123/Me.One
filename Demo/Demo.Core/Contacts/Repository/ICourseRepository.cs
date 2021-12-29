using System.Collections.Generic;
using Demo.Core.CQRS.Queries;
using Demo.Core.Models;
using Me.One.Core.Contract.Repository;

namespace Demo.Core.Contacts.Repository
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        List<Course> GetCourseByStudent(GetCourseByStudent query);
        Course GetCourseById(string id);
    }
}