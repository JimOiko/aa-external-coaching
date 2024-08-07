using AppointmentManagementSystem.Infastructure.Interfaces;
using AppointmentManagementSystem.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentManagementSystem.Interfaces;

namespace AppointmentManagementSystem.Services
{

    public class DiscountService : IDiscountService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly INameDayApiClient _namedayApiClient;

        public DiscountService(ICustomerRepository customerRepository, INameDayApiClient namedayApiClient)
        {
            _customerRepository = customerRepository;
            _namedayApiClient = namedayApiClient;
        }

        public async Task<bool> ProcessDiscountAsync(Guid customerId, DateTimeOffset appointmentDate)
        {
            // Define tasks for parallel execution
            var customerTask = _customerRepository.GetByIdAsync(customerId);
            var namedayTask = _namedayApiClient.GetNamedayAsync(appointmentDate);

            // Wait for both tasks to complete
            await Task.WhenAll(customerTask, namedayTask);

            var customer = await customerTask;
            var nameday = await namedayTask;

            // Check if the appointment date is a nameday
            if (customer != null)
                return nameday.Contains(customer.Name, StringComparison.OrdinalIgnoreCase);
            else
                throw new InvalidOperationException(); 
        }
    }
}
