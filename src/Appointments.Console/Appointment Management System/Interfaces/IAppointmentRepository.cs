using AppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Interfaces
{
    public interface IAppointmentRepository
    {
        #region CRUD
        void Add(Appointment item);
        List<Appointment> Get();
        Appointment? GetById(string id);
        void Delete(Appointment item);
        #endregion CRUD

        #region Reporting
        int GetCountByDate(DateTimeOffset date);
        int GetCountByType(ServiceType serviceType);
        MasseusePreference GetCommonPreferenceForMasseuseSex();
        TrainingDuration? GetCommonPreferenceForPTDuration();
        IEnumerable<ServiceTypeMaxAppointments> GetMaxAppointmentsDateByServiceType();
        MassageServices GetMassageTypePreference();
        (DayOfWeek Day, int Count) GetMaxAppointmentsDayOfWeek();
        (DayOfWeek Day, int Count) GetMinAppointmentsDayOfWeek();
        #endregion Reporting

    }
}
