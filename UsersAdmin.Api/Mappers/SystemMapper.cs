using UsersAdmin.Api.Dtos;
using UsersAdmin.Core.Models;
using AutoMapper;

namespace UsersAdmin.Api.Mappers
{
    public class SystemMapper : Profile
    {
        public SystemMapper()
        {
            CreateMap<SystemEntity, SystemDto>();
            CreateMap<SystemDto, SystemEntity>();
            CreateMap<SystemEntity, SystemEntity>();
        }
    }
}