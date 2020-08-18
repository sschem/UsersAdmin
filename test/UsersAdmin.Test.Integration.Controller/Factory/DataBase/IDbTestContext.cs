using Microsoft.Extensions.DependencyInjection;

namespace Tatisoft.UsersAdmin.Test.Integration.Controller.Factory.DataBase
{
    internal interface IDbTestContext
    {
        void Reset(IServiceScopeFactory scopeFactory);
    }
}
