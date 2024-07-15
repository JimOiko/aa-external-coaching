using AppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
