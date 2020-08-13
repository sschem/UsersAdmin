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
        public const string ADMIN_POLICY = "AdminPolicy";
        public const string USER_POLICY = "UserPolicy";
        public const string SYSTEM_ADMIN_POLICY = "SystemAdminPolicy";

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserRole.Admin.ToString())
                .Build();
        }

        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserRole.User.ToString())
                .Build();
        }

        public static AuthorizationPolicy SystemAdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserRole.SystemAdmin.ToString())
                //.RequireAssertion(context => context.User.HasClaim(c => c is List<Core.Model.System.SystemDto>));
                .Build();
        }
    }
}