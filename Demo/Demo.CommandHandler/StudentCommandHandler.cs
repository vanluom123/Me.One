using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Core.Contacts.Business;
using Demo.Core.CQRS.Commands;
using Kledex.Commands;
using Kledex.Events;

namespace Demo.CommandHandler
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