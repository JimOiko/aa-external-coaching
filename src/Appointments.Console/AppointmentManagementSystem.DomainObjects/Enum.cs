using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.DomainObjects
{
    public enum ServiceTypeEnum
    {
        Massage,
        PersonalTraining
    }

    public enum MassageServicesEnum
    {
        RelaxingMassage,
        HotStoneTherapy,
        Reflexology
    }

    public enum MasseusePreferenceEnum
    {
        Male,
        Female        
    }

    public enum TrainingDurationEnum
    {
        ThirtyMinutes,
        OneHour,
        OneHourThirtyMinutes
    }
}
