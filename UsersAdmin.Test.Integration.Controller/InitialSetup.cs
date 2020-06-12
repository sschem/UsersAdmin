using AutoMapper;
using UsersAdmin.Core.Model.Mapping;

namespace UsersAdmin.Test.Integration.Controller
{
    public static class InitialSetup
    {
        public static readonly IMapper MapperInstance;
        public static readonly ControllerAppFactory Factory;

        static InitialSetup()
        {
            Factory = new ControllerAppFactory(true);

            MapperInstance = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            })
            .CreateMapper();
        }
    }
}
