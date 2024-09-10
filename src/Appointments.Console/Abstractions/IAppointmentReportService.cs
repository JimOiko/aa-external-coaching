
using AppointmentManagementSystem.DomainObjects;

namespace AppointmentManagementSystem.Abstractions
{
    using Constants = AppointmentManagementSystem.DomainObjects.Enums;

    public interface IAppointmentReportService
    {
        Task<int> GetAppointmentsCountByDateAsync(DateTimeOffset date);
        Task<(int massageCount, int ptCount)> GetNumberOfAppointmentsByTypeAsync();
        Task<Constants.MasseusePreference> GetCommonPreferenceForMasseuseSexAsync();
        Task<Constants.TrainingDuration?> GetCommonPreferenceForTrainingDurationAsync();
        Task<IEnumerable<ServiceTypeMaxAppointments?>> GetMaxAppointmentsDateByServiceTypeAsync();
        Task<Constants.MassageServices> GetMassageTypePreferenceAsync();
        Task<(DayOfWeek MaxDay, int MaxCount, DayOfWeek MinDay, int MinCount)> GetAppointmentsDayOfWeekReportAsync();

    }
}
