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
        void Add(T item);
        List<T> Get();
        T? GetById(string id);
        void Delete(T item);
    }
}
