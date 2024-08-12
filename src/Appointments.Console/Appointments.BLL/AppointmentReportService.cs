using Appointments.DAL.Interfaces;
using Appointments.BLL.Interfaces;

namespace AppointmentManagementSystem.Services
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public class AppointmentReportService(IAppointmentRepository appointmentRepo): IAppointmentReportService
    {
        private readonly IAppointmentRepository _appointmentRepo = appointmentRepo;

        public async Task GetAppointmentsCountByDateAsync(DateTimeOffset date)
        {
            var appointmentCount = await _appointmentRepo.GetCountByDateAsync(date);
            Console.WriteLine($"Total Number of Appointments on {date.DateTime.ToShortDateString()}: {appointmentCount}");
        }

        public async Task GetNumberOfAppointmentsByTypeAsync()
        {
            var massageCount = await _appointmentRepo.GetCountByTypeAsync(AllEnums.ServiceType.Massage);
            var ptCount = await _appointmentRepo.GetCountByTypeAsync(AllEnums.ServiceType.PersonalTraining);
            Console.WriteLine($"Total Number of Massage Appointments: {massageCount}");
            Console.WriteLine($"Total Number of Persontal Training Appointments: {ptCount}");
        }

        public async Task GetCommonPreferenceForMasseuseSexAsync()
        {
            var commonPreference = await _appointmentRepo.GetCommonPreferenceForMasseuseSexAsync();
            Console.WriteLine($"Most Common Preference for Masseuse Sex: {commonPreference}");
        }
        public async Task GetCommonPreferenceForTrainingDurationAsync()
        {
            var commonPreference = await _appointmentRepo.GetCommonPreferenceForPTDurationAsync();
            if( commonPreference != null )
                Console.WriteLine($"Most Common Preference for Personal Training Duration: {commonPreference}");
            else
                Console.WriteLine("Not enough Data to extract this Stat");
        }

        public async Task GetMaxAppointmentsDateByServiceTypeAsync()
        {
            var maxAppointmentsByServiceType = await _appointmentRepo.GetMaxAppointmentsDateByServiceTypeAsync();

            foreach (var maxAppointment in maxAppointmentsByServiceType)
            {
                Console.WriteLine($"Service Type: {maxAppointment.ServiceType}");
                if (maxAppointment.Count != 0)
                {
                    Console.WriteLine($"Date with Max Appointments: {maxAppointment.Date?.DateTime.ToShortDateString()}, Count: {maxAppointment?.Count}");
                }
                else
                {
                    Console.WriteLine("No appointments found for this service type.");
                }
            }
        }

        public async Task GetMassageTypePreferenceAsync()
        {
            var massageTypePref = await _appointmentRepo.GetMassageTypePreferenceAsync();
            Console.WriteLine($"Most Common Preference for Massage Service: {massageTypePref}");
        }

        public async Task GetAppointmentsDayOfWeekReportAsync()
        {
            var maxAppointmentsTask = _appointmentRepo.GetMaxAppointmentsDayOfWeekAsync();
            var minAppointmentsTask = _appointmentRepo.GetMinAppointmentsDayOfWeekAsync();

            await Task.WhenAll(maxAppointmentsTask, minAppointmentsTask);

            var (maxDay, maxCount) = maxAppointmentsTask.Result;
            var (minDay, minCount) = minAppointmentsTask.Result;

            Console.WriteLine($"Day with Maximum Appointments: {maxDay}, Count: {maxCount}");
            Console.WriteLine($"Day with Minimum Appointments: {minDay}, Count: {minCount}");
        }

    }
}
