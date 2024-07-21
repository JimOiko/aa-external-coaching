
namespace AppointmentManagementSystem.Interfaces
{
    public interface IAppointmentReportService
    {
        void GetAppointmentsCountByDate(DateTime date);
        void GetNumberOfAppointmentsByType();
        void GetCommonPreferenceForMasseuseSex();
        void GetCommonPreferenceForTrainingDuration();
        void GetMaxAppointmentsDateByServiceType();
        void GetMassageTypePreference();
        void GetAppointmentsDayOfWeekReport();

    }
}
