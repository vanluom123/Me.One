using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Commands;
using Kledex.Events;
using MediatorTutorials.Core.Contacts.Business;
using MediatorTutorials.Core.CQRS.Commands;

namespace MediatorTutorials.CommandHander
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