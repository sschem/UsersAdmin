using AutoMapper;
using Tatisoft.UsersAdmin.Core.Model.Mapping;

namespace Tatisoft.UsersAdmin.Core.Model.User
{
    public class UserLoginDto : DtoBase, IMapFrom<UserEntity>
    {
        public string Id { get; set; }
        public string Pass { get; set; }
    }
}
