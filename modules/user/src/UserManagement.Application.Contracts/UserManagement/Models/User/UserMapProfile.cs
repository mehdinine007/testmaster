using AutoMapper;
using UserManagement.Application.Contracts;
using UserManagement.Domain.Authorization.Users;

namespace UserManagement.Application.Contracts.Models;

public class UserMapProfile : Profile
{
    public UserMapProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<UserDto, User>()
            .ForMember(x => x.Roles, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore());

        CreateMap<CreateUserDto, User>();
        CreateMap<CreateUserDto, User>();
        CreateMap<UserMongo, UserMongoETO>().ForMember(x => x.MongoId, opt => opt.MapFrom(y => y.Id.ToString())).IgnoreAllSourcePropertiesWithAnInaccessibleSetter().IgnoreAllPropertiesWithAnInaccessibleSetter();            



        CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());


   



       
           


    }
}
