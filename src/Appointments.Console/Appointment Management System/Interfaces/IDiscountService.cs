using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Interfaces
{
    public interface IDiscountService
    {
        Task<bool> ProcessDiscountAsync(Guid customerId, DateTimeOffset appointmentDate);
    }
}
