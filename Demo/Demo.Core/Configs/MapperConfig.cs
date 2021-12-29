using AutoMapper;

namespace Demo.Core.Configs
{
    public static class MapperConfig
    {
        public static IMapper Init()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                StudentMapperConfig.CreateMap(cfg);
                CourseMapperConfig.CreateMap(cfg);
            });

            return new Mapper(mapperConfig);
        }
    }
}