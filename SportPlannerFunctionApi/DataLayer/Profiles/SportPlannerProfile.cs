using AutoMapper;
using ModelsCore;
using SportPlannerIngestion.DataLayer.Models;
using System;

namespace SportPlannerFunctionApi.DataLayer.Profiles
{
    public class SportPlannerProfile : Profile
    {
        public SportPlannerProfile()
        {
            #region DtoToModel

            CreateMap<EventUserDto, EventUser>()
                .ForMember(dest => dest.EventId, opt => opt.Ignore())
                .ForMember(dest => dest.Event, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<EventDto, Event>()
                .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Address != null ? (Guid?)src.Address.Id : null))
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    foreach (var item in dest.Users)
                    {
                        item.EventId = src.Id;
                    }
                });

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Events, opt => opt.Ignore());

            CreateMap<AddressDto, Address>();

            #endregion DtoToModel

            #region ModelToDto

            CreateMap<Event, EventDto>();
            CreateMap<EventUser, EventUserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));

            CreateMap<User, UserDto>();
            CreateMap<Address, AddressDto>();

            #endregion ModelToDto
        }
    }
}
