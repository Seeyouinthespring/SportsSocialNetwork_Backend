using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SportsSocialNetwork.Attributes
{
    public class ServerErrorResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!operation.Responses.ContainsKey("500"))
                operation.Responses.Add("500", new OpenApiResponse { Description = "Internal server error" });
        }
    }
}
