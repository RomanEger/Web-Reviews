﻿using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace WebReviews.API.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Videostatus, ReferenceDTO>()
                .ForMember("id", x => x.MapFrom(a => a.VideoStatusId));
            CreateMap<ReferenceForManipulationDTO, Videostatus>()
                .ReverseMap();

            CreateMap<Userrank, ExtentedReferenceDTO>()
                .ForMember("id", x => x.MapFrom(a => a.UserRankId));
            CreateMap<ExtentedReferenceForManipDTO, Userrank>();

            CreateMap<UserForUpdateDTO, User>().ReverseMap();
            CreateMap<User, UserDTO>();
            CreateMap<UserForRegistrationDTO, User>().ReverseMap();

            CreateMap<VideoForManipulationDTO, Video>().ReverseMap();
            CreateMap<Video, VideoDTO>();

            CreateMap<Usercomment, UserCommentDTO>();
            CreateMap<UserCommentForManipulationDTO, Usercomment>().ReverseMap();

            CreateMap<Videorating, VideoRatingDTO>();
            CreateMap<VideoRatingForManipulationDTO, Videorating>().ReverseMap();
        }
    }
}
