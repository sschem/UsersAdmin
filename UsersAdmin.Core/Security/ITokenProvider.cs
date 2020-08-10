using UsersAdmin.Core.Model.User;

namespace UsersAdmin.Core.Security
{
    public interface ITokenProvider
    {
        string BuildToken(UserLoggedDto user);
    }
}
