using AutoMapper;
using UsersAdmin.Core.Model.Mapping;

namespace UsersAdmin.Core.Model.User
{
    public class UserLoggedDto : DtoBase, IMapFrom<UserEntity>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserEntity, UserLoggedDto>()
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.ToString()));
        }
    }
}
