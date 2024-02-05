using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace WebReviews.API.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Videostatus, ReferenceDTO>()
                .ForMember("id", x=> x.MapFrom(a => a.VideoStatusId));
            CreateMap<ReferenceForManipulationDTO, Videostatus>()
                .ReverseMap();
        }
    }
}
