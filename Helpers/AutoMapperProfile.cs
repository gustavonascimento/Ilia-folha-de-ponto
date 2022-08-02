using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Allocations;
using WebApi.Models.Moment;
using WebApi.Models.NewAccess;
using WebApi.Models.Users;

namespace WebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<RegisterNewAccess, NewAccessUser>();
            CreateMap<NewAccessUser, NewAccessUsersDto>();
            CreateMap<AllocationModel, Allocation>();
            CreateMap<CreateAllocationModel, Allocation>();
            CreateMap<CreateMomentModel, Moment>();

        }
    }
}