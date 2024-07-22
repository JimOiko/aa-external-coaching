using AppointmentManagementSystem.Interfaces;
using AppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Repositories
{
    public class CustomerRepository: IManagementRepository<Customer>
    {
        private readonly List<Customer> _customers =[];

        public void Add(Customer customer)
        {
            _customers.Add(customer);
        }

        public List<Customer> Get()
        {
            return (List<Customer>)_customers.Clone();
        }

        public Customer? GetById(string id)
        {
            var existingCustomer = _customers.FirstOrDefault(c => c.Email.Equals(id, StringComparison.OrdinalIgnoreCase));
            return existingCustomer;
        }

        public void Delete(Customer customer)
        {
            _customers.Remove(customer);
        }
    }
}
