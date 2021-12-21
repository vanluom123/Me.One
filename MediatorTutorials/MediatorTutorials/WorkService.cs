using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kledex;
using MediatorTutorials.Core.CQRS.Commands;
using MediatorTutorials.Core.CQRS.Queries;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MediatorTutorials
{
    public class WorkService : IHostedService
    {
        private readonly IDispatcher _dispatcher;
        private readonly IMapper _mapper;

        public WorkService(IDispatcher dispatcher, IMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            GetStudentsByCourse();
            GetStudents();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #region Student

        private void CreateStudent()
        {
            var student = new CreateOrUpdateStudent
            {
                Id = "S004",
                FirstName = "Isaac",
                LastName = "Newton",
                DoB = "04/01/1643",
                Classmate = "12C6",
                Gender = "Male",
                Deleted = false
            };

            _dispatcher.SendAsync(student).Wait();
        }

        private void GetStudents()
        {
            var studentQuery = new GetStudent();
            var getStudents = _dispatcher.GetResultAsync(studentQuery);
            var result = getStudents.Result;

            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using var sw = new StreamWriter(@"d:\ListStudents.txt");
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, result.Items);
        }

        private void GetStudentsByCourse()
        {
            var studentQuery = new GetStudentByCourse { Id = "C003" };
            var getStudents = _dispatcher.GetResultAsync(studentQuery);
            var result = getStudents.Result;

            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using var sw = new StreamWriter(@"d:\StudentByCourse.txt");
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, result.Items);
        }

        #endregion

        #region Course

        private void CreateCourse()
        {
            var course = new CreateCourse { Id = "C003", Name = "Software Open-source" };
            _dispatcher.SendAsync(course).Wait();
        }

        private void GetCourses()
        {
            var course = new GetCourses();
            var result = _dispatcher.GetResultAsync(course).Result;

            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using var sw = new StreamWriter(@"d:\ListCourses.txt");
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, result.Items);
        }

        private void GetListCoursesByStudent()
        {
            var course = new GetCourseByStudent { Id = "S002" };
            var result = _dispatcher.GetResultAsync(course).Result;

            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using var sw = new StreamWriter(@"d:\ListCoursesByStudent.txt");
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, result.Items);
        }

        #endregion
    }
}