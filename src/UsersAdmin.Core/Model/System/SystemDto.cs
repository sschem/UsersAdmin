using AutoMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Tatisoft.UsersAdmin.Core.Model.Mapping;
using Tatisoft.UsersAdmin.Core.Model.User;

namespace Tatisoft.UsersAdmin.Core.Model.System
{
    public class SystemDto : DtoBase, IMapFrom<SystemEntity>
    {
        public SystemDto()
        {
            this.Users = new List<UserItemDto>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public List<UserItemDto> Users { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SystemEntity, SystemDto>()
                .ForMember(dto => dto.Users,
                    ent => ent.MapFrom(e => e.UserSystemLst.Any() ? e.UserSystemLst.Select(us => us.User) : null))
                .AfterMap((ent, dto) => dto.Users = dto.Users.Any() ? dto.Users : null);
            ;
        }
    }
}
