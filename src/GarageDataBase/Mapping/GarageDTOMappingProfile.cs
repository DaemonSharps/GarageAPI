using AutoMapper;
using GarageDataBase.DTO;
using GarageDataBase.Tables;

namespace GarageDataBase.Mapping;

public class GarageDTOMappingProfile : Profile
{
    public GarageDTOMappingProfile()
    {
        CreateMap<UserTable, User>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.UserState.Name));

        CreateMap<RecordTable, Record>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RecordState.Name));
    }
}

