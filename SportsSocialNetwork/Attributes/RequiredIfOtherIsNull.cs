using Microsoft.EntityFrameworkCore.Internal;
using SportsSocialNetwork.Business.Constants;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SportsSocialNetwork.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RequiredIfOtherIsNull : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public RequiredIfOtherIsNull(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Type containerType = validationContext.ObjectInstance.GetType();
            System.Reflection.PropertyInfo field = containerType.GetProperty(_otherPropertyName);
            object fieldValue = (object)field.GetValue(validationContext.ObjectInstance, null);

            if (fieldValue != null)
                return ValidationResult.Success;

            if (value == null)
                return new ValidationResult(Validation.REQUIRED_TYPE);

            var collection = value as IEnumerable;

            if (collection == null)
                return ValidationResult.Success;

            if (collection.Any())
                return ValidationResult.Success;
            return new ValidationResult(Validation.REQUIRED_TYPE);
        }
    }
}
