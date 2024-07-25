﻿using AppManagementSystem.DbObjects;
using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Infastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManagementSystem.Infastructure
{
    public class AppointmentRepository(AppointmentManagementContext db) : IAppointmentRepository
    {

        #region CRUD
        public void Add(Appointment appointment)
        {
            // Check if the customer is already being tracked
            var trackedCustomer = db.Customer.Local.FirstOrDefault(c => c.Id == appointment.Customer.Id);
            if (trackedCustomer != null)
            {
                // Use the tracked entity instead of the new instance
                appointment.Customer = trackedCustomer;
            }
            else
            {
                // Attach the customer to the context if not already tracked
                db.Customer.Attach(appointment.Customer);
            }
            db.Appointment.Add(appointment);
            db.SaveChanges();
        }

        public List<Appointment> Get()
        {
            return [.. db.Appointment.Include(a => a.Customer)];
        }

        public Appointment? GetById(string id)
        {
            int idInt;
            if (!int.TryParse(id, out idInt))
            {
                // Handle invalid id here if necessary
                return null;
            }
            var existingCustomer = db.Appointment.FirstOrDefault(a => a.AppointmentId == idInt);
            return existingCustomer;
        }

        public void Update(Appointment appointment)
        {
            db.Appointment.Update(appointment);
            db.SaveChanges();
        }

        public void Delete(Appointment appointment)
        {
            db.Appointment.Remove(appointment);
            db.SaveChanges();
        }
        #endregion CRUD

        #region Reporting
        public int GetCountByDate(DateTimeOffset date)
        {
            return db.Appointment.Count(a => a.Date.Date == date.Date);
        }

        public int GetCountByType(ServiceType serviceType)
        {
            return db.Appointment.Where(a=>a.ServiceType == serviceType).Count();
        }

        public MasseusePreference GetCommonPreferenceForMasseuseSex()
        {
            return db.Appointment
                .OfType<MassageAppointment>()
                .Where(a => a.ServiceType == ServiceType.Massage)
                .GroupBy(a => a.Preference)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }

        public TrainingDuration? GetCommonPreferenceForPTDuration()
        {

            return db.Appointment
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
                 var appointment = db.Appointment
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
            return db.Appointment
                .OfType<MassageAppointment>()
                .Where(a => a.ServiceType == ServiceType.Massage)
                .GroupBy(a => a.MassageServices)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }

        public (DayOfWeek Day, int Count) GetMaxAppointmentsDayOfWeek()
        {
            var groupedByDayOfWeek = db.Appointment.AsEnumerable() //Not the best solution need to reconsider
                .GroupBy(a => a.Date.DayOfWeek)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefault();

            return groupedByDayOfWeek != null ? (groupedByDayOfWeek.Day, groupedByDayOfWeek.Count) : (default, 0);
        }

        public (DayOfWeek Day, int Count) GetMinAppointmentsDayOfWeek()
        {
            var groupedByDayOfWeek = db.Appointment.AsEnumerable()
                .GroupBy(a => a.Date.DayOfWeek)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .OrderBy(g => g.Count)
                .FirstOrDefault();

            return groupedByDayOfWeek != null ? (groupedByDayOfWeek.Day, groupedByDayOfWeek.Count) : (default, 0);
        }

        #endregion Reporting
    }
}
