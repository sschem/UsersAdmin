using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAdmin.Core.Model.User;

namespace UsersAdmin.Api.Auth
{
    public class Policies
    {
        public const string ADMIN_ROLE = "Admin";
        public const string USER_ROLE = "User";

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ADMIN_ROLE)
                .Build();
        }

        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(USER_ROLE)
                .Build();
        }
    }
}