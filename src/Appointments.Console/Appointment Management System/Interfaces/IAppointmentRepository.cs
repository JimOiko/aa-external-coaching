using AppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Interfaces
{
    public interface IAppointmentRepository
    {
        void Add(Appointment item);
        List<Appointment> Get();
        Appointment? GetById(string id);
        void Delete(Appointment item);
        int GetCountByDate(DateTime date);
    }
}
