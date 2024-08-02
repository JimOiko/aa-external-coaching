using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.DomainObjects.Enums
{
    public enum ServiceType
    {
        Massage,
        PersonalTraining
    }

    public enum MassageServices
    {
        RelaxingMassage,
        HotStoneTherapy,
        Reflexology
    }

    public enum MasseusePreference
    {
        Male,
        Female        
    }

    public enum TrainingDuration
    {
        ThirtyMinutes,
        OneHour,
        OneHourThirtyMinutes
    }
}
