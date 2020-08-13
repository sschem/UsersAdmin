using UsersAdmin.Core.Model.User;

namespace UsersAdmin.Core.Security
{
    public interface ITokenProvider
    {
        TokenInfo BuildToken(UserEntity user, string systemId);
    }
}
