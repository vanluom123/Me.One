using AutoMapper;
using Demo.Core.CQRS.Commands;
using Demo.Core.CQRS.Result;
using Demo.Core.Models;

namespace Demo.Core.Configs
{
    public static class StudentMapperConfig
    {
        public static void CreateMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Student, GetStudentResult>()
                .ReverseMap()
                .ConstructUsing(x => new Student(x.Id));

            cfg.CreateMap<CreateOrUpdateStudent, Student>()
                .ConstructUsing(x => new Student(x.Id))
                .ForMember(dest => dest.FullName, act => act.MapFrom(src => src.FirstName + " " + src.LastName))
                .ReverseMap();
        }
    }
}