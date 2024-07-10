using AppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Interfaces
{
    public interface IManagementRepository<T>
    {
        void Create(List<T> items, List<Customer>? item = null);
        void Read(List<T> items);
        void Update(List<T> items);
        void Delete(List<T> items);
    }
}
