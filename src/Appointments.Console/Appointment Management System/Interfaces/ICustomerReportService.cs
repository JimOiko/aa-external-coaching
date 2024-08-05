using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Interfaces
{
    public interface ICustomerReportService
    {
        Task GetRegisteredCustomerAsync();
        Task GetNewCustomersByDateAsync(DateTimeOffset date);

    }
}
