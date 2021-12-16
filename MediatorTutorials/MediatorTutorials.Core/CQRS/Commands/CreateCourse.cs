using Me.One.Core.CQRS.Models;

namespace MediatorTutorials.Core.CQRS.Commands
{
    public class CreateCourse : BaseCommand
    {
        public CreateCourse()
        {
            ValidateCommand = false;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}