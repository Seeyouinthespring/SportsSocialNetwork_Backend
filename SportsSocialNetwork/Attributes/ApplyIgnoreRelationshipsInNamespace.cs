using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SportsSocialNetwork.Attributes
{
    /// <summary>
    /// Apply this filter to ignore all related objects that could cause  a self referencing loop
    /// </summary>
    public class ApplyIgnoreRelationshipsInNamespace : ISchemaFilter
    {
        private const string DOMAIN_MODEL_NAME_SPACE = "ICT4Kraam.Database.Domain.Model";

        /// <summary>
        /// Required by interface
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (model.Properties == null)
                return;
            var excludeList = new List<string>();

            if (context.Type.Namespace == DOMAIN_MODEL_NAME_SPACE)
            {
                excludeList.AddRange(
                    from prop in context.Type.GetProperties()
                    where prop.PropertyType.Namespace == DOMAIN_MODEL_NAME_SPACE
                    select prop.Name);
            }

            foreach (var prop in excludeList)
            {
                if (model.Properties.ContainsKey(prop))
                    model.Properties.Remove(prop);
            }
        }
    }
}
