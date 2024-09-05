using AppointmentManagementSystem.DomainObjects.Interfaces;

namespace AppointmentManagementSystem.DomainObjects
{
    public class UserInputService : IUserInputService
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
