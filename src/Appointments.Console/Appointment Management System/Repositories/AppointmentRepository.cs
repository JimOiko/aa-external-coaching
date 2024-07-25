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
        public int GetCountByDate(DateTimeOffset date)
        {
            return _appointments.Count(a => a.Date.Date == date.Date);
        }

        public int GetCountByType(ServiceType serviceType)
        {
            return _appointments.Where(a => a.ServiceType == serviceType).Count();
        }

        public MasseusePreference GetCommonPreferenceForMasseuseSex()
        {
            return _appointments
                .OfType<MassageAppointment>()
                .Where(a => a.ServiceType == ServiceType.Massage)
                .GroupBy(a => a.Preference)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }

        public TrainingDuration? GetCommonPreferenceForPTDuration()
        {

            return _appointments
                .OfType<PersonalTrainingAppointment>()
                .Where(a => a.ServiceType == ServiceType.PersonalTraining)
                .GroupBy(a => a.TrainingDuration)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }

        public IEnumerable<ServiceTypeMaxAppointments> GetMaxAppointmentsDateByServiceType()
        {

            return Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>()
             .Select(serviceType =>
             {
                 var appointment = _appointments
                     .Where(a => a.ServiceType == serviceType)
                     .GroupBy(a => a.Date.Date)
                     .OrderByDescending(g => g.Count())
                     .Select(g => new { Date = g.Key, Count = g.Count() })
                     .FirstOrDefault();

                 return new ServiceTypeMaxAppointments
                 {
                     ServiceType = serviceType,
                     Date = appointment?.Date,
                     Count = appointment?.Count ?? 0
                 };
             })
             .ToList();
        }

        public MassageServices GetMassageTypePreference()
        {
            return _appointments
                .OfType<MassageAppointment>()
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
