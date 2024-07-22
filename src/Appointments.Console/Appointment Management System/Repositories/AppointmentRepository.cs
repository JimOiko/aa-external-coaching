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

        public int GetCountByType(ServiceType serviceType)
        {
            return _appointments.Where(a=>a.ServiceType == serviceType).Count();
        }

        public MasseusePreference GetCommonPreferenceForMasseuseSex()
        {
            var massageAppointments = _appointments
           .OfType<MassageAppointment>()
           .ToList();

            return massageAppointments
                .Where(a => a.ServiceType == ServiceType.Massage)
                .GroupBy(a => a.Preference)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }

        public TrainingDuration? GetCommonPreferenceForPTDuration()
        {
            var ptAppointments = _appointments
           .OfType<PersonalTrainingAppointment>()
           .ToList();

            if (ptAppointments == null)
                return null;

            return ptAppointments
                .Where(a => a.ServiceType == ServiceType.PersonalTraining)
                .GroupBy(a => a.TrainingDuration)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }

        public Dictionary<ServiceType, (DateTime? Date, int Count)> GetMaxAppointmentsDateByServiceType()
        {
            var result = new Dictionary<ServiceType, (DateTime? Date, int Count)>();

            var serviceTypes = Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>();

            foreach (var serviceType in serviceTypes)
            {
                var groupedByDate = _appointments
                    .Where(a => a.ServiceType == serviceType)
                    .GroupBy(a => a.Date.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .FirstOrDefault();

                result[serviceType] = groupedByDate != null ? (groupedByDate.Date, groupedByDate.Count) : (null, 0);
            }

            return result;
        }

        public MassageServices GetMassageTypePreference()
        {
            var massageAppointments = _appointments
           .OfType<MassageAppointment>()
           .ToList();
            return massageAppointments
                .Where(a => a.ServiceType == ServiceType.Massage)
                .GroupBy(a => a.MassageServices)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }

        public (DayOfWeek Day, int Count) GetMaxAppointmentsDayOfWeek()
        {
            var groupedByDayOfWeek = _appointments
                .GroupBy(a => a.Date.DayOfWeek)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefault();

            return groupedByDayOfWeek != null ? (groupedByDayOfWeek.Day, groupedByDayOfWeek.Count) : (default, 0);
        }

        public (DayOfWeek Day, int Count) GetMinAppointmentsDayOfWeek()
        {
            var groupedByDayOfWeek = _appointments
                .GroupBy(a => a.Date.DayOfWeek)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .OrderBy(g => g.Count)
                .FirstOrDefault();

            return groupedByDayOfWeek != null ? (groupedByDayOfWeek.Day, groupedByDayOfWeek.Count) : (default, 0);
        }

        #endregion Reporting
    }
}
