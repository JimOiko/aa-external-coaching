using Appointments.DAL.Interfaces;
using AppointmentManagementSystem.Abstractions;
using Appointments.BLL.Interfaces;

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

            Task? allTasks = null;
            try
            {
                allTasks = Task.WhenAll(customerTask, namedayTask);
                await allTasks; // awaiting the task here 
                var customer = await customerTask;
                var nameday = await namedayTask;

                if (customer != null)
                {
                    return nameday.Contains(customer.Name, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception ex)
            {
                AggregateException? exception = allTasks?.Exception;
                foreach (var innerException in exception?.InnerExceptions)
                {
                    if (innerException is InvalidOperationException)
                    {
                        // Handle InvalidOperationException specifically
                        throw new InvalidOperationException("Customer not found.");
                    }
                    else if (innerException is HttpRequestException)
                    {
                        // Handle HTTP request errors (likely from the NameDay API call)
                        throw new HttpRequestException("Error occurred while calling the NameDay API.");
                    }
                    else
                    {
                        // Handle other types of exceptions
                        throw new InvalidOperationException("An unknown error occurred.");
                    }
                }

                throw;
            }
        }
    }
}
