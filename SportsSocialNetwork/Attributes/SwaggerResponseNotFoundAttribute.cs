using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Attributes
{
    public class SwaggerResponseNotFoundAttribute : SwaggerResponseAttribute
    {
        public SwaggerResponseNotFoundAttribute() : base((int)HttpStatusCode.NotFound, description: "Object not found")
        {
        }
    }
}
