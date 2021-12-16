using Me.One.Core.CQRS.Models;

namespace MediatorTutorials.Core.CQRS.Commands
{
    public class CreateOrUpdateStudent : BaseCommand
    {
        public CreateOrUpdateStudent()
        {
            ValidateCommand = false;
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DoB { get; set; }
        public string Gender { get; set; }
        public string Classmate { get; set; }
        public bool Deleted { get; set; }
    }
}