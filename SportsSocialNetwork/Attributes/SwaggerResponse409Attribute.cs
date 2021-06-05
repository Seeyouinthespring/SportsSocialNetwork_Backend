using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace SportsSocialNetwork.Attributes
{
    public class SwaggerResponse409Attribute : SwaggerResponseAttribute
    {
        protected SwaggerResponse409Attribute(params Dictionary<string, string>[] errorCodes) : base((int)HttpStatusCode.Conflict, type: typeof(ConflictErrorModel))
        {
            IEnumerable<KeyValuePair<string, string>> resultCodes = Array.Empty<KeyValuePair<string, string>>();
            errorCodes = errorCodes.Where(x => x != null).ToArray();
            foreach (Dictionary<string, string> codes in errorCodes)
            {
                resultCodes = resultCodes.Union(codes);
            }

            Description = "Conflict on the server.<br>" + string.Join(";<br>", resultCodes.Select(x => x.Key + " - " + x.Value));
        }
    }

    /// <summary>
    /// Conflict error description
    /// </summary>
    public class ConflictErrorModel
    {
        /// <summary>
        /// Conflict code
        /// </summary>
        /// <example>DeletionProhibited</example>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        /// <example>SomeFieldId</example>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
