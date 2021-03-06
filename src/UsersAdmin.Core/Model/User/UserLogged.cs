﻿using AutoMapper;
using Tatisoft.UsersAdmin.Core.Model.Mapping;

namespace Tatisoft.UsersAdmin.Core.Model.User
{
    public class UserLoggedDto : DtoBase, IMapFrom<UserEntity>
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Token { get; set; }
        
        public string Role { get; set; }
    }
}
