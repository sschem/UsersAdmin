using AutoMapper;
using UsersAdmin.Core.Model.Mapping;

namespace UsersAdmin.Core.Model.System
{
    public class SystemItemDto : DtoBase, IMapFrom<SystemEntity>
    {
        public string SystemId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SystemEntity, SystemItemDto>()
                .ForMember(d => d.SystemId, opt => opt.MapFrom(s => s.Id));
        }
    }
}
