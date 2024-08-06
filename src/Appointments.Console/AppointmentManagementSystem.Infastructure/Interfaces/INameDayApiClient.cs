using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Infastructure.Interfaces
{
    public interface INameDayApiClient
    {
        Task<string> GetNamedayAsync(DateTimeOffset date);
    }
}
