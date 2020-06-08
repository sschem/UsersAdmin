using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UsersAdmin.Core.Model.Mapping;
using UsersAdmin.Core.Model.System;
using UsersAdmin.Core.Model.User;
using UsersAdmin.Core.Repositories;

namespace UsersAdmin.Test.Unit.Service
{
    public class Testing
    {
        public static readonly IMapper MapperInstance;

        static Testing()
        {
            MapperInstance = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            })
            .CreateMapper();
        }

        public static SystemDto GetValidSystemDto() => new SystemDto()
        {
            Id = "SystemValidId",
            Name = "System Valid Name",
            Description = "System Valid Description"
        };

        public static UserDto GetValidUserDto() => new UserDto()
        {
            Id = "UserValidId",
            Name = "User Valid Name",
            Description = "User Valid Description",
            Email = "validuser@mail.com",
            Pass = "validclearpass"
        };
    }
}
