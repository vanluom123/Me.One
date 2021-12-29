using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Core.Contacts.Business;
using Demo.Core.CQRS.Commands;
using Kledex.Commands;
using Kledex.Events;

namespace Demo.CommandHandler
{
    public class CourseCommandHandler : ICommandHandlerAsync<CreateCourse>
    {
        private readonly ICourseBusiness _courseBusiness;

        public CourseCommandHandler(ICourseBusiness courseBusiness)
        {
            _courseBusiness = courseBusiness;
        }

        public async Task<CommandResponse> HandleAsync(CreateCourse command)
        {
            await _courseBusiness.Create(command);
            return await Task.FromResult(new CommandResponse
            {
                Events = new List<IEvent>()
            });
        }
    }
}