using System;
using AutoMapper;

namespace GarageDataBase.Mapping
{
    public static class MapperHelper
    {
        public static IMapper CreateMapper()
            => new MapperConfiguration(c
                => c.AddProfile<GarageDTOMappingProfile>())
            .CreateMapper();
    }
}

