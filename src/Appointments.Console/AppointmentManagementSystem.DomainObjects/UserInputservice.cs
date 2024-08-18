using AppointmentManagementSystem.DomainObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
