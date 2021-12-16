using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Me.One.Core.CQRS.Exceptions;
using Me.One.Core.CQRS.Models;

namespace Me.One.Core.CQRS
{
    public abstract class BaseCommandValidator<T> : AbstractValidator<T>, ICommandValidator<T>
        where T : BaseCommand
    {
        public void ValidateCommand(T command)
        {
            var validationResult =
                command != null ? Validate(command) : throw new ArgumentNullException(nameof(command));
            if (!validationResult.IsValid)
                throw new InvalidValidationException(validationResult.Errors);
        }

        public IList<FieldError> GetCommandErrors(T command)
        {
            var validationResult =
                command != null ? Validate(command) : throw new ArgumentNullException(nameof(command));
            return validationResult.IsValid
                ? new List<FieldError>()
                : (IList<FieldError>) validationResult.Errors.Select((Func<ValidationFailure, FieldError>) (item =>
                    new FieldError
                    {
                        PropertyName = item.PropertyName,
                        ErrorCode = item.ErrorCode,
                        ErrorMessage = item.ErrorMessage
                    })).ToList();
        }

        protected virtual void CustomRules()
        {
        }
    }
}