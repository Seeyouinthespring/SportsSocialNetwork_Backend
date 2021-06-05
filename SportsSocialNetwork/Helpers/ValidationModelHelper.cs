using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace SportsSocialNetwork.Helpers
{
    public static class ValidationModelHelper
    {
        public static string FirstCharToLower(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return input.First().ToString().ToLower() + input.Substring(1);
        }

        public static string Join(this IEnumerable<string> values, string separator = "")
        {
            return string.Join(separator, values);
        }

        public static JsonResult GenerateErrorMessage(ModelStateDictionary modelState) // TO DO change on Bad Request
        {
            var errors = modelState
                        .Where(x => x.Value.ValidationState == ModelValidationState.Invalid)
                        .Select(x => {
                            var errorMessage = x.Value.Errors.FirstOrDefault()?.ErrorMessage;
                            if (errorMessage != Validation.INVALID_TYPE && errorMessage != Validation.REQUIRED_TYPE)
                                errorMessage = Validation.INVALID_TYPE; // For some convert value or other errors, not validation

                            return new ValidationErrorDescriptionModel
                            {
                                Field = x.Key.Split(".").Select(f => f.FirstCharToLower()).Join("."),
                                Error = errorMessage
                            };
                        })
                        .ToArray();
            return new JsonResult(new ValidationErrorModel
            {
                Code = Validation.INVALID_VALIDATION_CODE,
                Fields = errors
            });
        }
    }

    public static class Validation 
    {
        public const string REQUIRED_TYPE = "REQUIRED";
        public const string INVALID_TYPE = "INVALID";
        public const string INVALID_VALIDATION_CODE = "VALIDATION";
    }

    public struct ValidationErrorDescriptionModel
    {
        /// <summary>
        /// Field name
        /// </summary>
        /// <example>momentId, factorDetails[1].lipId</example>
        public string Field { get; set; }
        /// <summary>
        /// Error type
        /// </summary>
        /// <example>REQUIRED, INVALID</example>
        public string Error { get; set; }
    }

    public struct ValidationErrorModel
    {
        /// <summary>
        /// Validation code
        /// </summary>
        /// <example>VALIDATION</example>
        public string Code { get; set; }

        /// <summary>
        /// Errors description
        /// </summary>
        public ValidationErrorDescriptionModel[] Fields { get; set; }
    }
}
