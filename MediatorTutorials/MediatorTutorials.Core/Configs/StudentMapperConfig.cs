using AutoMapper;
using MediatorTutorials.Core.CQRS.Commands;
using MediatorTutorials.Core.CQRS.Result;
using MediatorTutorials.Core.Models;

namespace MediatorTutorials.Core.Configs
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