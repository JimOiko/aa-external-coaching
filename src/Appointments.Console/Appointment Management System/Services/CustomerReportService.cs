using AppointmentManagementSystem.Infastructure.Interfaces;
using AppointmentManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Services
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
