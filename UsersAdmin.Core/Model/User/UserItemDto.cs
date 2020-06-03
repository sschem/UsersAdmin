﻿using AutoMapper;
using UsersAdmin.Core.Model.Mapping;

namespace UsersAdmin.Core.Model.User
{
    public class UserItemDto : DtoBase, IMapFrom<UserEntity>
    {
        public string UserId { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserEntity, UserItemDto>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Id));
        }
    }
}
