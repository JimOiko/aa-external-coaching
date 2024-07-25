
namespace AppointmentManagementSystem.Interfaces
{
    public interface IAppointmentReportService
    {
        void GetAppointmentsCountByDate(DateTimeOffset date);
        void GetNumberOfAppointmentsByType();
        void GetCommonPreferenceForMasseuseSex();
        void GetCommonPreferenceForTrainingDuration();
        void GetMaxAppointmentsDateByServiceType();
        void GetMassageTypePreference();
        void GetAppointmentsDayOfWeekReport();

    }
}
