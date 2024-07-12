using AppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Interfaces
{
    public interface IDataEntryServiceFactory
    {
        IDataEntryService Create(DataEntryServiceType serviceType);
    }

}
