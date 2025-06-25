using AutoMapper;
using BoxBoxApi.DTOs;
using BoxBoxModels;

namespace BoxBoxApi.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.ProfilePicture,
                    opt => opt.Ignore());

            CreateMap<UserRequestDto, User>();
        }
       
    }
}
