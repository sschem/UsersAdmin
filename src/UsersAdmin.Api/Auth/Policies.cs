using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tatisoft.UsersAdmin.Core.Model.User;

namespace Tatisoft.UsersAdmin.Api.Auth
{
    public class Policies
    {
        public const string ADMIN_POLICY = "AdminPolicy";
        public const string SYSTEM_ADMIN_POLICY = "SystemAdminPolicy";
        public const string USER_POLICY = "UserPolicy";

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserRole.Admin.ToString())
                .Build();
        }

        public static AuthorizationPolicy SystemAdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserRole.SystemAdmin.ToString(), UserRole.Admin.ToString())
                //TODO: access to httpContext for validate system (claim.systemId == httpCtx.systemId)
                .RequireAssertion(context => 
                    context.User.IsInRole(UserRole.Admin.ToString()) || 
                    context.User.HasClaim(c => c.Type == "systemId"))
                .Build();
        }

        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(UserRole.User.ToString(), UserRole.SystemAdmin.ToString(), UserRole.Admin.ToString())
                //TODO: access to httpContext for validate system (claim.systemId == httpCtx.systemId)
                .RequireAssertion(context =>
                    context.User.IsInRole(UserRole.Admin.ToString()) ||
                    context.User.HasClaim(c => c.Type == "systemId"))
                .Build();
        }
    }
}