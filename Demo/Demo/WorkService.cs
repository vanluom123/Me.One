using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Core.CQRS.Commands;
using Demo.Core.CQRS.Queries;
using Kledex;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Demo
{
    public class WorkService : IHostedService
    {
        private readonly IDispatcher _dispatcher;
        private readonly IMapper _mapper;

        private delegate void Function();

        private List<Function> _listFunctions;

        public WorkService(IDispatcher dispatcher, IMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
            _listFunctions = new List<Function> { 
                CreateStudent, 
                CreateCourse, 
                GetStudents, 
                GetStudentsByCourse,
                GetCourses,
                GetListCoursesByStudent
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _listFunctions[2]();
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

            using var sw = new StreamWriter(string.Format("{0}ListStudents.txt", FileOperator.GetPath()));
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

            using var sw = new StreamWriter(string.Format("{0}StudentByCourse.txt", FileOperator.GetPath()));
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

            using var sw = new StreamWriter(string.Format("{0}ListCourses.txt", FileOperator.GetPath()));
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

            using var sw = new StreamWriter(string.Format("{0}ListCoursesByStudent.txt", FileOperator.GetPath()));
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, result.Items);
        }

        #endregion
    }
}