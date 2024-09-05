using AppointmentManagementSystem.DbObjects;
using AppointmentManagementSystem.DomainObjects;
using Microsoft.EntityFrameworkCore;
using AppointmentManagementSystem.Abstractions;

namespace Appointments.DAL
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public class AppointmentRepository(IDbContextFactory dbFactory) : IAppointmentRepository
    {

        #region CRUD
        public async Task AddAsync(Appointment appointment)
        {
            using var db = dbFactory.CreateDbContext();
            db.Appointment.Add(appointment);
            await db.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetAsync()
        {
            using var db = dbFactory.CreateDbContext();
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
            using var db = dbFactory.CreateDbContext();

            return await db.Appointment.FirstOrDefaultAsync(a => a.AppointmentId == idInt);
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            using var db = dbFactory.CreateDbContext();
            db.Appointment.Update(appointment);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            using var db = dbFactory.CreateDbContext();
            db.Appointment.Remove(appointment);
            await db.SaveChangesAsync();
        }
        #endregion CRUD

        #region Reporting
        public async Task<int> GetCountByDateAsync(DateTimeOffset date)
        {
            using var db = dbFactory.CreateDbContext();
            return await db.Appointment.CountAsync(a => a.Date.Date == date.Date);
        }

        public async Task<int> GetCountByTypeAsync(AllEnums.ServiceType serviceType)
        {
            using var db = dbFactory.CreateDbContext();
            return await db.Appointment.Where(a => a.ServiceType == serviceType).CountAsync();
        }

        public async Task<AllEnums.MasseusePreference> GetCommonPreferenceForMasseuseSexAsync()
        {
            using var db = dbFactory.CreateDbContext();
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

            using var db = dbFactory.CreateDbContext();
            return await db.Appointment
                .OfType<PersonalTrainingAppointment>()
                .Where(a => a.ServiceType == AllEnums.ServiceType.PersonalTraining)
                .GroupBy(a => a.TrainingDuration)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ServiceTypeMaxAppointments?>> GetMaxAppointmentsDateByServiceTypeAsync()
        {
            using var db = dbFactory.CreateDbContext();

            return await db.Appointment.GroupBy(a => new { a.ServiceType, a.Date.Date }).Select(g => new
            {
                g.Key.ServiceType,
                g.Key.Date,
                Count = g.Count()
            }).GroupBy(g => g.ServiceType)
            .Select(g => g.OrderByDescending(x => x.Count)
                .Select(s => new ServiceTypeMaxAppointments
                {
                    ServiceType = s.ServiceType,
                    Date = s.Date,
                    Count = s.Count
                }).FirstOrDefault())
            .ToListAsync();

        }

        public async Task<AllEnums.MassageServices> GetMassageTypePreferenceAsync()
        {
            using var db = dbFactory.CreateDbContext();
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
            using var db = dbFactory.CreateDbContext();
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
            using var db = dbFactory.CreateDbContext();
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
