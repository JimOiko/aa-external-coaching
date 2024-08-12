using AppointmentManagementSystem.DomainObjects;

namespace Appointments.DAL.Interfaces
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public interface IAppointmentRepository
    {
        #region CRUD
        Task AddAsync(Appointment item);
        Task<List<Appointment>> GetAsync();
        Task<Appointment?> GetByIdAsync(string id);
        Task UpdateAsync(Appointment item);
        Task DeleteAsync(Appointment item);
        #endregion CRUD

        #region Reporting
        Task<int> GetCountByDateAsync(DateTimeOffset date);
        Task<int> GetCountByTypeAsync(AllEnums.ServiceType serviceType);
        Task<AllEnums.MasseusePreference> GetCommonPreferenceForMasseuseSexAsync();
        Task<AllEnums.TrainingDuration?> GetCommonPreferenceForPTDurationAsync();
        Task<IEnumerable<ServiceTypeMaxAppointments?>> GetMaxAppointmentsDateByServiceTypeAsync();
        Task<AllEnums.MassageServices> GetMassageTypePreferenceAsync();
        Task<(DayOfWeek Day, int Count)> GetMaxAppointmentsDayOfWeekAsync();
        Task<(DayOfWeek Day, int Count)> GetMinAppointmentsDayOfWeekAsync();
        #endregion Reporting

    }
}
