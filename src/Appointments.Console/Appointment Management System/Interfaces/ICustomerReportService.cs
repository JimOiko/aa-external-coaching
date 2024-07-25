using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Interfaces
{
    public interface ICustomerReportService
    {
        void GetRegisteredCustomer();
        void GetNewCustomersByDate(DateTimeOffset date);

    }
}
