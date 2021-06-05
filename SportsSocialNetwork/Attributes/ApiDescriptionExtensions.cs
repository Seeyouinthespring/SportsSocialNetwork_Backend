using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Attributes
{
    public static class ApiDescriptionExtensions
    {
        public static IList<string> GroupBySwaggerGroupAttribute(this ApiDescription api)
        {
            // ------
            // Lifted from ApiDescriptionExtensions
            var actionDescriptor = api.GetProperty<ControllerActionDescriptor>();

            api.TryGetMethodInfo(out MethodInfo methodInfo);
            var groupNameAttribute = (SwaggerGroupAttribute)methodInfo.DeclaringType.GetCustomAttributes().SingleOrDefault(attribute => attribute is SwaggerGroupAttribute);

            if (actionDescriptor == null)
            {
                actionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                api.SetProperty(actionDescriptor);
            }
            // ------

            string name = groupNameAttribute != null ? groupNameAttribute.GroupName : actionDescriptor?.ControllerName;

            return new[] { name };
        }
    }

    public class SwaggerGroupAttribute : Attribute
    {
        public string GroupName { get; }

        public SwaggerGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}
