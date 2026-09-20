using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Infrastructure.Attributes;

namespace Infrastructure.Configuration.Filters
{
    public class ValidationFilter(IServiceProvider serviceProvider) : IActionFilter
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            MethodInfo? methodInfo = null;

            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                methodInfo = controllerActionDescriptor.MethodInfo;
            }

            if (methodInfo == null)
            {
                return;
            }

            var validationDescriptors = GetValidators(methodInfo, _serviceProvider);

            foreach (var descriptor in validationDescriptors)
            {
                var argument = context.ActionArguments[descriptor.ArgumentName];

                if (argument is not null)
                {
                    var validationResult = descriptor.Validator.Validate(new ValidationContext<object>(argument));

                    if (!validationResult.IsValid)
                    {
                        var errors = validationResult.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(e => e.ErrorMessage).ToList()
                            );

                        context.Result = new UnprocessableEntityObjectResult(new { errors });
                        return;
                    }
                }
            }
        }

        static IEnumerable<ValidationDescriptor> GetValidators(MethodBase methodInfo, IServiceProvider serviceProvider)
        {
            var parameters = methodInfo.GetParameters();

            for (var index = 0; index < parameters.Length; index++)
            {
                var parameter = parameters[index];

                if (parameter.GetCustomAttribute<ValidateAttribute>() is not null)
                {
                    var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

                    var validator = serviceProvider.GetService(validatorType) as IValidator;

                    if (validator is not null)
                    {
                        yield return new ValidationDescriptor { ArgumentName = parameter.Name!, ArgumentType = parameter.ParameterType, Validator = validator };
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }
    }

    public class ValidationDescriptor
    {
        public string ArgumentName { get; set; } = default!;
        public Type ArgumentType { get; set; } = default!;
        public IValidator Validator { get; set; } = default!;
    }
}
