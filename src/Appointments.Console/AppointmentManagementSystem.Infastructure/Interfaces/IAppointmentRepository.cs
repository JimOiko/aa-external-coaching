using AppointmentManagementSystem.DomainObjects;

namespace AppointmentManagementSystem.Infastructure.Interfaces
{
    public interface IAppointmentRepository
    {
        #region CRUD
        void Add(Appointment item);
        List<Appointment> Get();
        Appointment? GetById(string id);
        void Update(Appointment item);
        void Delete(Appointment item);
        #endregion CRUD

        #region Reporting
        int GetCountByDate(DateTimeOffset date);
        int GetCountByType(ServiceTypeEnum serviceType);
        MasseusePreferenceEnum GetCommonPreferenceForMasseuseSex();
        TrainingDurationEnum? GetCommonPreferenceForPTDuration();
        IEnumerable<ServiceTypeMaxAppointments> GetMaxAppointmentsDateByServiceType();
        MassageServicesEnum GetMassageTypePreference();
        (DayOfWeek Day, int Count) GetMaxAppointmentsDayOfWeek();
        (DayOfWeek Day, int Count) GetMinAppointmentsDayOfWeek();
        #endregion Reporting

    }
}
