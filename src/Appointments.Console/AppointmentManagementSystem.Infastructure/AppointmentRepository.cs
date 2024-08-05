using AppManagementSystem.DbObjects;
using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Infastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManagementSystem.Infastructure
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public class AppointmentRepository(AppointmentManagementContext db) : IAppointmentRepository
    {

        #region CRUD
        public async Task AddAsync(Appointment appointment)
        {
            await db.Appointment.AddAsync(appointment);
            await db.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAsync()
        {
            return await db.Appointment.ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(string id)
        {
            Guid idInt;
            if (!Guid.TryParse(id, out idInt))
            {
                // Handle invalid id here if necessary
                return null;
            }
            
            return await db.Appointment.FirstOrDefaultAsync(a => a.AppointmentId == idInt);
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            await Task.Run(() => db.Appointment.Update(appointment));
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            await Task.Run(() => db.Appointment.Remove(appointment));
            await db.SaveChangesAsync();
        }
        #endregion CRUD

        #region Reporting
        public async Task<int> GetCountByDateAsync(DateTimeOffset date)
        {
            return await db.Appointment.CountAsync(a => a.Date.Date == date.Date);
        }

        public async Task<int> GetCountByTypeAsync(AllEnums.ServiceType serviceType)
        {
            return await db.Appointment.Where(a=>a.ServiceType == serviceType).CountAsync();
        }

        public async Task<AllEnums.MasseusePreference> GetCommonPreferenceForMasseuseSexAsync()
        {
            return await db.Appointment
                .OfType<MassageAppointment>()
                .Where(a => a.ServiceType == AllEnums.ServiceType.Massage)
                .GroupBy(a => a.Preference)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();
        }

        public async Task<AllEnums.TrainingDuration?> GetCommonPreferenceForPTDurationAsync()
        {

            return await db.Appointment
                .OfType<PersonalTrainingAppointment>()
                .Where(a => a.ServiceType == AllEnums.ServiceType.PersonalTraining)
                .GroupBy(a => a.TrainingDuration)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ServiceTypeMaxAppointments>> GetMaxAppointmentsDateByServiceTypeAsync()
        {

            return await Task.WhenAll(Enum.GetValues(typeof(AllEnums.ServiceType)).Cast<AllEnums.ServiceType>()
             .Select(async serviceType =>
             {
                 var appointment = await db.Appointment
                     .Where(a => a.ServiceType == serviceType)
                     .GroupBy(a => a.Date.Date)
                     .OrderByDescending(g => g.Count())
                     .Select(g => new { Date = g.Key, Count = g.Count() })
                     .FirstOrDefaultAsync();

                 return new ServiceTypeMaxAppointments
                 {
                     ServiceType = serviceType,
                     Date = appointment?.Date,
                     Count = appointment?.Count ?? 0
                 };
             }));
        }

        public async Task<AllEnums.MassageServices> GetMassageTypePreferenceAsync()
        {
            return await db.Appointment
                .OfType<MassageAppointment>()
                .Where(a => a.ServiceType == AllEnums.ServiceType.Massage)
                .GroupBy(a => a.MassageServices)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();
        }

        public async Task<(DayOfWeek Day, int Count)> GetMaxAppointmentsDayOfWeekAsync()
        {
            // Retrieve all appointments from the database
            var appointments = await db.Appointment.ToListAsync();

            // Group by DayOfWeek in memory
            var groupedByDayOfWeek = appointments
                .GroupBy(a => a.Date.DayOfWeek)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefault();

            return groupedByDayOfWeek != null ? (groupedByDayOfWeek.Day, groupedByDayOfWeek.Count) : (default, 0);
        }

        public async Task<(DayOfWeek Day, int Count)> GetMinAppointmentsDayOfWeekAsync()
        {
            // Retrieve all appointments from the database
            var appointments = await db.Appointment.ToListAsync();

            // Group by DayOfWeek in memory
            var groupedByDayOfWeek = appointments
                .GroupBy(a => a.Date.DayOfWeek)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .OrderBy(g => g.Count)
                .FirstOrDefault();

            return groupedByDayOfWeek != null ? (groupedByDayOfWeek.Day, groupedByDayOfWeek.Count) : (default, 0);
        }

        #endregion Reporting
    }
}
