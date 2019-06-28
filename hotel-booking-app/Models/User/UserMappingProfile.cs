using AutoMapper;

namespace HotelBookingApp.Models.User
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserLoginReq, UserModel>();
            CreateMap<UserSignupReq, UserModel>();
        }
    }
}
