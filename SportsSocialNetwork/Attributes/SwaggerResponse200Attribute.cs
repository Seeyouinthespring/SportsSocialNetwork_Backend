using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace SportsSocialNetwork.Attributes
{
    public class SwaggerResponse200Attribute : SwaggerResponseAttribute
    {
        public SwaggerResponse200Attribute(Type type) : base((int)HttpStatusCode.OK, type: type)
        {
        }
    }

    public class SwaggerResponseNoContentAttribute : SwaggerResponseAttribute
    {
        public SwaggerResponseNoContentAttribute() : base((int)HttpStatusCode.NoContent)
        {
        }
    }

}
