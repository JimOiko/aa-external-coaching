using AppointmentManagementSystem.DomainObjects;

namespace AppointmentManagementSystem.Abstractions
{
    using Constants = AppointmentManagementSystem.DomainObjects.Enums;
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
        Task<int> GetCountByTypeAsync(Constants.ServiceType serviceType);
        Task<Constants.MasseusePreference> GetCommonPreferenceForMasseuseSexAsync();
        Task<Constants.TrainingDuration?> GetCommonPreferenceForPTDurationAsync();
        Task<IEnumerable<ServiceTypeMaxAppointments?>> GetMaxAppointmentsDateByServiceTypeAsync();
        Task<Constants.MassageServices> GetMassageTypePreferenceAsync();
        Task<(DayOfWeek Day, int Count)> GetMaxAppointmentsDayOfWeekAsync();
        Task<(DayOfWeek Day, int Count)> GetMinAppointmentsDayOfWeekAsync();
        #endregion Reporting

    }
}
