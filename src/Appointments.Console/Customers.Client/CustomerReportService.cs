using AppointmentManagementSystem.Abstractions;

namespace Customers.Client
{
    public class CustomerReportService(ICustomerRepository customerRepo): ICustomerReportService
    {
        private readonly ICustomerRepository _customerRepo = customerRepo;

        public async Task GetRegisteredCustomerAsync()
        {
            var customerCount = await _customerRepo.GetCountAsync();
            Console.WriteLine($"Total Number of Registered Customers: {customerCount}");
        }

        public async Task GetNewCustomersByDateAsync(DateTimeOffset date)
        {
            var newCustomers = await _customerRepo.GetNewCustomersByDateAsync(date);
            Console.WriteLine($"New Customers Registered on {date.DateTime.ToShortDateString()}:");
            if (newCustomers.Count == 0)
            {
                Console.WriteLine("No new customers registered on this date.");
            }
            else
            {
                foreach (var customer in newCustomers)
                {
                    Console.WriteLine(customer);
                }
            }
        }

    }
}
