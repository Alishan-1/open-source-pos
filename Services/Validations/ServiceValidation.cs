using Models;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Validations
{
    class ServiceValidation
    {
        public static ServiceResponse Validate<T>(T model, AbstractValidator<T> validator)
        {
            ServiceResponse vmServiceResponse = new ServiceResponse();
            ValidationResult results = validator.Validate(model);

            vmServiceResponse.IsValid = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;

            var errorDictionary =
            failures.GroupBy(x => x.PropertyName)
           .ToDictionary(
               g => g.Key,
               g => g.Select(x => x.ErrorMessage).ToList()
            );
            vmServiceResponse.Errors = errorDictionary;
            return vmServiceResponse;
        }
    }
}
