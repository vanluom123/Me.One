using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Me.One.Core.Business;
using Me.One.Core.CQRS.Models;
using Me.One.Core.Data;
using MediatorTutorials.Core.Contacts.Business;
using MediatorTutorials.Core.Contacts.Repository;
using MediatorTutorials.Core.CQRS.Commands;
using MediatorTutorials.Core.CQRS.Queries;
using MediatorTutorials.Core.CQRS.Result;
using MediatorTutorials.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MediatorTutorials.Business
{
    public class StudentBusiness : BaseBusiness<Student>, IStudentBusiness
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;

        public StudentBusiness(IStudentRepository studentRepo, IMapper mapper)
            : base(studentRepo)
        {
            _mapper = mapper;
            _studentRepository = studentRepo;
        }

        public async Task<ListResult<GetStudentResult>> GetAllStudents(GetStudent query)
        {
            var result = new ListResult<GetStudentResult>();
            var students = _studentRepository
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .List()
                .ToList();
            result.Items = _mapper.Map<List<GetStudentResult>>(students);
            return await Task.FromResult(result);
        }

        public async Task<ListResult<GetStudentResult>> GetStudentByCourse(GetStudentByCourse query)
        {
            var result = new ListResult<GetStudentResult>();
            var students = _studentRepository.GetStudentByCourse(query);
            result.Items = _mapper.Map<List<GetStudentResult>>(students);
            return await Task.FromResult(result);
        }

        public async Task CreateOrUpdate(CreateOrUpdateStudent command)
        {
            if (string.IsNullOrEmpty(command.Id))
            {
                var student = _mapper.Map<Student>(command);
                _studentRepository.Insert(student, true);
            }
            else
            {
                var entity = _studentRepository.GetById(command.Id);
                if (entity == null) throw new NullReferenceException(nameof(entity));
                entity = _mapper.Map<Student>(command);
                _studentRepository.Update(entity, true);
            }

            await Task.CompletedTask;
        }
    }
}