using AutoMapper;

namespace GarageDataBase.Mapping
{
    public static class MapperHelper
    {
        public static IMapper CreateMapper()
            => new MapperConfiguration(c
                => c.AddProfile<GarageDTOMappingProfile>())
            .CreateMapper();

        public static TDestination Map<TDestination>(object source)
        {
            var mapper = CreateMapper();
            return mapper.Map<TDestination>(source);
        }
    }
}

