using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace Me.One.Core.CQRS.Exceptions
{
    public class InvalidValidationException : System.Exception
    {
        public readonly IList<FieldError> Errors;

        public InvalidValidationException(IList<ValidationFailure> errors)
        {
            Errors = errors.Select((Func<ValidationFailure, FieldError>) (item => new FieldError
            {
                PropertyName = item.PropertyName,
                ErrorCode = item.ErrorCode,
                ErrorMessage = item.ErrorMessage
            })).ToList();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Errors);
        }
    }
}