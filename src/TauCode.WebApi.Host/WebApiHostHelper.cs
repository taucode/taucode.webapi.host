﻿using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.WebApi.Host
{
    internal static class WebApiHostHelper
    {
        internal static ErrorDto ToErrorDto(this Exception exception, string code = null)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var errorDto = new ErrorDto(code ?? exception.GetType().FullName, exception.Message);
            return errorDto;
        }

        internal static ValidationErrorDto CreateValidationErrorDto(string message, IEnumerable<ValidationFailure> failures)
        {
            if (failures == null)
            {
                throw new ArgumentNullException(nameof(failures));
            }

            var validationError = ValidationErrorDto.CreateStandard(message);

            foreach (var validationFailure in failures)
            {
                // Make sure that all the property names are camel cased
                var propertyName = EnsurePropertyNameIsCamelCase(validationFailure.PropertyName);

                // Only add the first validation message for a property
                if (!validationError.Failures.ContainsKey(propertyName))
                {
                    validationError.AddFailure(propertyName, validationFailure.ErrorCode, validationFailure.ErrorMessage);
                }
            }

            return validationError;
        }

        internal static ValidationErrorDto CreateValidationErrorDto(ValidationResult validationResult)
        {
            if (validationResult == null)
            {
                throw new ArgumentNullException(nameof(validationResult));
            }

            return CreateValidationErrorDto(null, validationResult.Errors);
        }

        internal static ValidationErrorDto CreateValidationErrorDto(ValidationException validationException)
        {
            if (validationException == null)
            {
                throw new ArgumentNullException(nameof(validationException));
            }

            return CreateValidationErrorDto(validationException.Message, validationException.Errors);
        }

        /// <summary>
        /// Since ModelState and FluentValidation use different casing (Pascal vs Camel) for their property names,
        /// this method is used to ensure that the property name is camel case.
        /// </summary>
        /// <param name="propertyName">The property name to convert (e.g. MyProperty.AnotherProperty)</param>
        /// <returns>The converted property (e.g. myProperty.anotherProperty)</returns>
        internal static string EnsurePropertyNameIsCamelCase(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                propertyName = "[unknown]";
            }

            // Find all nested property names
            var nestedPropertyNames = propertyName.Split('.');

            var sb = new StringBuilder();

            // Convert all nested property names into camel case
            for (var i = 0; i < nestedPropertyNames.Length; i++)
            {
                var nestedPropertyName = nestedPropertyNames[i];
                sb.Append(char.ToLowerInvariant(nestedPropertyName[0]) + nestedPropertyName.Substring(1));

                // Do not append a period if there are no more nested properties
                if (i + 1 < nestedPropertyNames.Length)
                {
                    sb.Append('.');
                }
            }

            return sb.ToString();
        }
    }
}
