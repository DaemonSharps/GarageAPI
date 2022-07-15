using AutoMapper;
using GarageDataBase.DTO;
using GarageDataBase.Tables;

namespace GarageDataBase.Mapping;

public class GarageDTOMappingProfile : Profile
{
    public GarageDTOMappingProfile()
    {
        CreateMap<CustomerTable, Customer>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.CustomerState.Name));
    }
}

