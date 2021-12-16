using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Commands;
using Kledex.Events;
using MediatorTutorials.Core.Contacts.Business;
using MediatorTutorials.Core.CQRS.Commands;

namespace MediatorTutorials.CommandHander
{
    public class StudentCommandHandler : ICommandHandlerAsync<CreateOrUpdateStudent>
    {
        private readonly IStudentBusiness _studentBusiness;

        public StudentCommandHandler(IStudentBusiness studentBusiness)
        {
            _studentBusiness = studentBusiness;
        }

        public async Task<CommandResponse> HandleAsync(CreateOrUpdateStudent command)
        {
            await _studentBusiness.CreateOrUpdate(command);
            return await Task.FromResult(new CommandResponse {Events = new List<IEvent>()});
        }
    }
}