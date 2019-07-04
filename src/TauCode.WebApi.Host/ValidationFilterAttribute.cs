using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TauCode.Cqrs.Commands;
using TauCode.Validation;

namespace TauCode.WebApi.Host
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly Dictionary<Type, Type> _validatorTypes; // key is validated type (e.g. FooCommand), value is validator (e.g. FooCommandValidator)

        public ValidationFilterAttribute(Assembly coreAssembly)
        {
            if (coreAssembly == null)
            {
                throw new ArgumentNullException(nameof(coreAssembly));
            }

            _validatorTypes = coreAssembly
                .GetTypes()
                .Where(IsCommandValidator)
                .ToDictionary(GetValidatedType, x => x);
        }

        private static Type GetValidatedType(Type validatorType)
        {
            return validatorType.BaseType.GetGenericArguments().Single();
        }

        private static bool IsCommandValidator(Type type)
        {
            var baseType = type.BaseType;

            if (baseType == null)
            {
                return false; // interface or something
            }

            if (baseType.IsGenericType)
            {
                var generic = baseType.GetGenericTypeDefinition();
                if (generic == typeof(AbstractValidator<>))
                {
                    var arg = baseType.GetGenericArguments().Single();
                    var argInterfaces = arg.GetInterfaces();
                    if (!argInterfaces.Contains(typeof(ICommand)))
                    {
                        return false;
                    }

                    return true;
                }

                return false;
            }

            return false;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            //var requestScope = actionContext.HttpContext.Request.GetDependencyScope();

            ValidationErrorResponseDto validationErrorResponse = null;

            // Verify that the model state is valid before running the argument value specific validators.
            if (!actionContext.ModelState.IsValid)
            {
                // Prepare the validation error response
                validationErrorResponse = ValidationErrorResponseDto.Standard;

                foreach (var fieldState in actionContext.ModelState)
                {
                    // Make sure that all the property names are camel cased and remove all model prefixes
                    var fieldName = EnsurePropertyNameIsCamelCase(Regex.Replace(fieldState.Key, @"^(.*?\.)(.*)", "$2"));

                    // Get only the first error, the rest will be skipped
                    var error = fieldState.Value.Errors.First();

                    // Create the error message
                    var errorMessage = "Unknown error";
                    if (!string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        errorMessage = error.ErrorMessage;
                    }
                    else if (!string.IsNullOrEmpty(error.Exception?.Message))
                    {
                        errorMessage = error.Exception.Message;
                    }

                    // Add the error to the response, with an empty error code, since this is an unspecific error
                    validationErrorResponse.WithValidationError(fieldName, null, errorMessage);
                }
            }

            // Validate all the arguments for the current action
            foreach (var argument in actionContext.ActionDescriptor./*GetParameters()*/Parameters)
            {
                // Skip all arguments without a registered validator
                _validatorTypes.TryGetValue(argument.ParameterType, out var validatorType);
                if (validatorType == null)
                {
                    continue;
                }

                // Get the registered validator
                //var validator = (IValidator)requestScope.GetService(validatorType);
                var validator = (IValidator)actionContext.HttpContext.RequestServices.GetService(validatorType);

                if (validator == null)
                {
                    continue; // could not resolve validator
                }

                // Inject the action arguments into the validator, so that they can be used in the validation
                // This is a "hack" to, amongst other, support unique validation on the update commands where the resource id is needed to exclude itself from the unique check.
                if (validator is IParameterValidator)
                {
                    ((IParameterValidator)validator).Parameters = actionContext.ActionArguments;
                }

                // Validate the argument
                var argumentValue = actionContext.ActionArguments[argument./*ParameterName*/Name];

                if (argumentValue == null)
                {
                    validationErrorResponse = new ValidationErrorResponseDto
                    {
                        Message = $"Argument '{argument./*ParameterName*/Name}' is null",
                    };
                    break;
                }

                var validationResult = validator.Validate(argumentValue);

                // Return if the argument value was valid
                if (validationResult.IsValid)
                {
                    continue;
                }

                // Create an validation error response, if it does not already exist
                if (validationErrorResponse == null)
                {
                    validationErrorResponse = ValidationErrorResponseDto.Standard;
                }

                // Add every field specific error to validation error response
                foreach (var validationFailure in validationResult.Errors)
                {
                    // Make sure that all the property names are camel cased
                    var propertyName = EnsurePropertyNameIsCamelCase(validationFailure.PropertyName);

                    // Only add the first validation message for a property
                    if (!validationErrorResponse.Failures.ContainsKey(propertyName))
                    {
                        validationErrorResponse.WithValidationError(propertyName, validationFailure.ErrorCode, validationFailure.ErrorMessage);
                    }
                }
            }

            if (validationErrorResponse != null)
            {
                actionContext.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(validationErrorResponse),
                };

                // Set the action response to a 400 Bad Request, with the validation error response as content
                //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, validationErrorResponse);
                actionContext.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                actionContext.HttpContext.Response.Headers.Add(DtoHelper.SubReasonHeaderName, DtoHelper.ValidationErrorSubReason);
            }
        }

        /// <summary>
        /// Since ModelState and FluentValidation use different casing (Pascal vs Camel) for their property names,
        /// this method is used to ensure that the property name is camel case.
        /// </summary>
        /// <param name="propertyName">The property name to convert (e.g. MyProperty.AnotherProperty)</param>
        /// <returns>The converted property (e.g. myProperty.anotherProperty)</returns>
        private static string EnsurePropertyNameIsCamelCase(string propertyName)
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

        public Type[] GetCommandTypes() => _validatorTypes.Keys.ToArray();
    }
}
