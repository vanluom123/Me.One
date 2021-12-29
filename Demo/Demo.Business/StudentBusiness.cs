using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Core.Contacts.Business;
using Demo.Core.Contacts.Repository;
using Demo.Core.CQRS.Commands;
using Demo.Core.CQRS.Queries;
using Demo.Core.CQRS.Result;
using Demo.Core.Models;
using Me.One.Core.Business;
using Me.One.Core.CQRS.Models;

namespace Demo.Business
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
            var students = _studentRepository.GetListStudents();
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