using AppointmentManagementSystem.Infastructure.Interfaces;
using AppointmentManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Services
{
    public class CustomerReportService(ICustomerRepository customerRepo): ICustomerReportService
    {
        private readonly ICustomerRepository _customerRepo = customerRepo;

        public void GetRegisteredCustomer()
        {
            var customerCount = _customerRepo.GetCount();
            Console.WriteLine($"Total Number of Registered Customers: {customerCount}");
        }

        public void GetNewCustomersByDate(DateTime date)
        {
            var newCustomers = _customerRepo.GetNewCustomersByDate(date);
            Console.WriteLine($"New Customers Registered on {date.ToShortDateString()}:");
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
