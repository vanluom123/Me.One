using System.Collections.Generic;
using System.Linq;
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
    public class CourseBusiness : BaseBusiness<Course>, ICourseBusiness
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseBusiness(ICourseRepository baseRepository, IMapper mapper) : base(baseRepository)
        {
            _mapper = mapper;
            _courseRepository = baseRepository;
        }

        public async Task Create(CreateCourse command)
        {
            var course = _baseRepository.GetById(command.Id);
            if (course != null)
            {
                // Do something
            }
            else
            {
                var entity = _mapper.Map<Course>(command);
                _baseRepository.Insert(entity, true);
            }

            await Task.CompletedTask;
        }

        public async Task<ListResult<GetCourseResult>> GetAllCourses(GetCourses query)
        {
            var result = new ListResult<GetCourseResult>();
            var courses = _courseRepository.List().ToList();
            result.Items = _mapper.Map<List<GetCourseResult>>(courses);
            return await Task.FromResult(result);
        }

        public async Task<GetCourseResult> GetCourseById(GetCourse query)
        {
            var result = new GetCourseResult();
            var course = _courseRepository.GetCourseById(query.Id);
            result = _mapper.Map<GetCourseResult>(course);
            return await Task.FromResult(result);
        }

        public async Task<ListResult<GetCourseResult>> GetListCoursesByStudent(GetCourseByStudent query)
        {
            var result = new ListResult<GetCourseResult>();
            var courses = _courseRepository.GetCourseByStudent(query);
            result.Items = _mapper.Map<List<GetCourseResult>>(courses);
            return await Task.FromResult(result);
        }
    }
}