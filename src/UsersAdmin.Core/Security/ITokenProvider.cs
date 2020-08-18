using Tatisoft.UsersAdmin.Core.Model.User;

namespace Tatisoft.UsersAdmin.Core.Security
{
    public interface ITokenProvider
    {
        TokenInfo BuildToken(UserEntity user, string systemId = null);
    }
}
