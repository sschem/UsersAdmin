using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Tatisoft.UsersAdmin.Core.Model.Mapping;
using Tatisoft.UsersAdmin.Core.Model.System;
using Tatisoft.UsersAdmin.Core.Model.User;
using Tatisoft.UsersAdmin.Core.Repositories;

namespace Tatisoft.UsersAdmin.Test.Unit.Service
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

        public static UserLoginDto GetValidUserLoginDto() => new UserLoginDto()
        {
            Id = GetValidUserDto().Id,
            Pass = GetValidUserDto().Pass
        };
    }
}
