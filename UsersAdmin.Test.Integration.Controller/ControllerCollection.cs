using UsersAdmin.Test.Integration.Controller.Factory;
using Xunit;

namespace UsersAdmin.Test.Integration.Controller
{
    [CollectionDefinition("Controller collection")]
    public class ControllerCollection : ICollectionFixture<WebAppFactoryFixture>
    {

    }
}
