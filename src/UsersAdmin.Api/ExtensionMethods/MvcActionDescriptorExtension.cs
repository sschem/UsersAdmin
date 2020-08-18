namespace Tatisoft.UsersAdmin.Api.ExtensionMethods
{
    public static class MvcActionDescriptorExtension
    {
        public static string GetShortMethodName(this Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor actionDescriptor)
        {
            var splitedDisplayName = actionDescriptor.DisplayName.Split("(");
            var splitedMethodName = splitedDisplayName[0].Split(".");
            var shortMethodName = splitedMethodName[splitedMethodName.Length - 2].Trim() + "." + splitedMethodName[splitedMethodName.Length - 1].Trim();
            return shortMethodName;
        }
    }
}