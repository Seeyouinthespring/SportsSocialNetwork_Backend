using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using SportsSocialNetwork.Business.Constants;
using System.ComponentModel.DataAnnotations;

namespace SportsSocialNetwork.Helpers
{
    public class CustonValidationMetadataProvider : IValidationMetadataProvider
    {
        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (var attribute in context.ValidationMetadata.ValidatorMetadata)
            {
                ValidationAttribute tAttr = attribute as ValidationAttribute;

                if (tAttr == null) continue;

                tAttr.ErrorMessage = tAttr is RequiredAttribute
                    ? Validation.REQUIRED_TYPE
                    : Validation.INVALID_TYPE;
            }
        }
    }
}
