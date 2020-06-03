﻿using UsersAdmin.Core.Model.Mapping;

namespace UsersAdmin.Core.Model.User
{
    public class UserDto : DtoBase, IMapFrom<UserEntity>
    {
        public string Id { get; set; }
        public string Pass { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }
}