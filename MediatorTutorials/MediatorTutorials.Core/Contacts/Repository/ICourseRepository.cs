using System.Collections.Generic;
using Me.One.Core.Contract.Repository;
using MediatorTutorials.Core.CQRS.Queries;
using MediatorTutorials.Core.Models;

namespace MediatorTutorials.Core.Contacts.Repository
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        List<Course> GetCourseByStudent(GetCourseByStudent query);
    }
}