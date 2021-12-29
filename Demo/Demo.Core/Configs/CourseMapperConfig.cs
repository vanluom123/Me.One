using AutoMapper;
using Demo.Core.CQRS.Commands;
using Demo.Core.CQRS.Queries;
using Demo.Core.CQRS.Result;
using Demo.Core.Models;

namespace Demo.Core.Configs
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