using AppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Interfaces
{
    public interface ICustomerRepository
    {
        void Add(Customer item);
        List<Customer> Get();
        Customer? GetById(string id);
        void Delete(Customer item);
        int GetCount();
        List<Customer> GetNewCustomersByDate(DateTime date);
    }
}
