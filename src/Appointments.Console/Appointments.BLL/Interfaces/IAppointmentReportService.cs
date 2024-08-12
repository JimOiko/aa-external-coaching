
namespace Appointments.BLL.Interfaces
{
    public interface IAppointmentReportService
    {
        Task GetAppointmentsCountByDateAsync(DateTimeOffset date);
        Task GetNumberOfAppointmentsByTypeAsync();
        Task GetCommonPreferenceForMasseuseSexAsync();
        Task GetCommonPreferenceForTrainingDurationAsync();
        Task GetMaxAppointmentsDateByServiceTypeAsync();
        Task GetMassageTypePreferenceAsync();
        Task GetAppointmentsDayOfWeekReportAsync();

    }
}
