
using AppointmentManagementSystem.DomainObjects;

namespace AppointmentManagementSystem.Abstractions
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;

    public interface IAppointmentReportService
    {
        Task<int> GetAppointmentsCountByDateAsync(DateTimeOffset date);
        Task<(int massageCount, int ptCount)> GetNumberOfAppointmentsByTypeAsync();
        Task<AllEnums.MasseusePreference> GetCommonPreferenceForMasseuseSexAsync();
        Task<AllEnums.TrainingDuration?> GetCommonPreferenceForTrainingDurationAsync();
        Task<IEnumerable<ServiceTypeMaxAppointments?>> GetMaxAppointmentsDateByServiceTypeAsync();
        Task<AllEnums.MassageServices> GetMassageTypePreferenceAsync();
        Task<(DayOfWeek MaxDay, int MaxCount, DayOfWeek MinDay, int MinCount)> GetAppointmentsDayOfWeekReportAsync();

    }
}
