using System.Collections.Generic;
using Me.One.Core.CQRS.Exceptions;
using Me.One.Core.CQRS.Models;

namespace Me.One.Core.CQRS
{
    public interface ICommandValidator<T> where T : BaseCommand
    {
        void ValidateCommand(T command);

        IList<FieldError> GetCommandErrors(T command);
    }
}