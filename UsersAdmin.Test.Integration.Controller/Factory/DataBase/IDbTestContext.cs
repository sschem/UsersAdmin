using Microsoft.Extensions.DependencyInjection;

namespace UsersAdmin.Test.Integration.Controller.Factory.DataBase
{
    internal interface IDbTestContext
    {
        void Reset(IServiceScopeFactory scopeFactory);
    }
}
