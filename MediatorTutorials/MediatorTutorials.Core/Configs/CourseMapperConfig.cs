using AutoMapper;
using MediatorTutorials.Core.CQRS.Commands;
using MediatorTutorials.Core.CQRS.Queries;
using MediatorTutorials.Core.CQRS.Result;
using MediatorTutorials.Core.Models;

namespace MediatorTutorials.Core.Configs
{
    public static class CourseMapperConfig
    {
        public static void CreateMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<GetCourse, Course>()
                .ReverseMap();

            cfg.CreateMap<GetCourseResult, Course>()
                .ReverseMap();

            cfg.CreateMap<CreateCourse, Course>()
                .ConstructUsing(x => new Course(x.Id))
                .ReverseMap();
        }
    }
}