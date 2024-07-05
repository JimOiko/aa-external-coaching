using Appointment_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Management_System.Interfaces
{
    public interface IManagementRepository<T>
    {
        void Create(List<T> items, List<Customer>? item = null);
        void Read(List<T> items);
        void Update(List<T> items);
        void Delete(List<T> items);
    }
}
