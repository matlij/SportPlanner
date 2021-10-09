using AutoMapper;
using ModelsCore;
using SportPlannerIngestion.DataLayer.Models;

namespace SportPlannerApi.DataLayer.Profiles
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
