using AppointmentManagementSystem.Interfaces;
using AppointmentManagementSystem.Models;
using AppointmentManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Services
{
    public class AppointmentReportService(IAppointmentRepository appointmentRepo): IAppointmentReportService
    {
        private readonly IAppointmentRepository _appointmentRepo = appointmentRepo;

        public void GetAppointmentsCountByDate(DateTime date)
        {
            var appointmentCount = _appointmentRepo.GetCountByDate(date);
            Console.WriteLine($"Total Number of Appointments on {date.ToShortDateString()}: {appointmentCount}");
        }

        public void GetNumberOfAppointmentsByType()
        {
            var massageCount = _appointmentRepo.GetCountByType(ServiceType.Massage);
            var ptCount = _appointmentRepo.GetCountByType(ServiceType.PersonalTraining);
            Console.WriteLine($"Total Number of Massage Appointments: {massageCount}");
            Console.WriteLine($"Total Number of Persontal Training Appointments: {ptCount}");
        }

        public void GetCommonPreferenceForMasseuseSex()
        {
            var commonPreference = _appointmentRepo.GetCommonPreferenceForMasseuseSex();
            Console.WriteLine($"Most Common Preference for Masseuse Sex: {commonPreference}");
        }
        public void GetCommonPreferenceForTrainingDuration()
        {
            var commonPreference = _appointmentRepo.GetCommonPreferenceForPTDuration();
            Console.WriteLine($"Most Common Preference for Personal Training Duration: {commonPreference}");
        }

        public void GetMaxAppointmentsDateByServiceType()
        {
            var maxAppointmentsByServiceType = _appointmentRepo.GetMaxAppointmentsDateByServiceType();

            foreach (var serviceType in maxAppointmentsByServiceType.Keys)
            {
                var (date, count) = maxAppointmentsByServiceType[serviceType];
                Console.WriteLine($"Service Type: {serviceType}");
                if (date.HasValue)
                {
                    Console.WriteLine($"Date with Max Appointments: {date.Value.ToShortDateString()}, Count: {count}");
                }
                else
                {
                    Console.WriteLine("No appointments found for this service type.");
                }
            }
        }

        public void GetMassageTypePreference()
        {
            var massageTypePref = _appointmentRepo.GetMassageTypePreference();
            Console.WriteLine($"Most Common Preference for Massage Service: {massageTypePref}");
        }

        public void GetAppointmentsDayOfWeekReport()
        {
            var (maxDay, maxCount) = _appointmentRepo.GetMaxAppointmentsDayOfWeek();
            var (minDay, minCount) = _appointmentRepo.GetMinAppointmentsDayOfWeek();

            Console.WriteLine($"Day with Maximum Appointments: {maxDay}, Count: {maxCount}");
            Console.WriteLine($"Day with Minimum Appointments: {minDay}, Count: {minCount}");
        }

    }
}
