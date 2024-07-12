using AppointmentManagementSystem.Interfaces;
using AppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppointmentManagementSystem.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly List<Appointment> _appointments = [];

        #region CRUD
        public void Add(Appointment appointment)
        {
            _appointments.Add(appointment);
        }

        public List<Appointment> Get()
        {
            return (List<Appointment>)_appointments.Clone();
        }

        public Appointment? GetById(string id)
        {
            int idInt;
            if (!int.TryParse(id, out idInt))
            {
                // Handle invalid id here if necessary
                return null;
            }
            var existingCustomer = _appointments.FirstOrDefault(a => a.Id == idInt);
            return existingCustomer;
        }

        public void Delete(Appointment appointment)
        {
            _appointments.Remove(appointment);
        }
        #endregion CRUD

        #region Reporting
        public int GetCountByDate(DateTime date)
        {
            return _appointments.Count(a => a.Date.Date == date.Date);
        }
        #endregion Reporting
    }
}
