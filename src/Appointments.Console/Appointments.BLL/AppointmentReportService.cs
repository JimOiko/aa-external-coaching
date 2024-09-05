using AppointmentManagementSystem.Abstractions;
using AppointmentManagementSystem.DomainObjects;

namespace Appointments.BLL
{
    using Constants = AppointmentManagementSystem.DomainObjects.Enums;
    public class AppointmentReportService(IAppointmentRepository appointmentRepo): IAppointmentReportService
    {
        private readonly IAppointmentRepository _appointmentRepo = appointmentRepo;

        public async Task<int> GetAppointmentsCountByDateAsync(DateTimeOffset date)
        {
            var appointmentCount = await _appointmentRepo.GetCountByDateAsync(date);
            return appointmentCount;
        }

        public async Task<(int massageCount, int ptCount)> GetNumberOfAppointmentsByTypeAsync()
        {
            var massageCount = await _appointmentRepo.GetCountByTypeAsync(Constants.ServiceType.Massage);
            var ptCount = await _appointmentRepo.GetCountByTypeAsync(Constants.ServiceType.PersonalTraining);
            return (massageCount, ptCount);
        }

        public async Task<Constants.MasseusePreference> GetCommonPreferenceForMasseuseSexAsync()
        {
            var commonPreference = await _appointmentRepo.GetCommonPreferenceForMasseuseSexAsync();
            return commonPreference;
        }
        public async Task<Constants.TrainingDuration?> GetCommonPreferenceForTrainingDurationAsync()
        {
            var commonPreference = await _appointmentRepo.GetCommonPreferenceForPTDurationAsync();
            return commonPreference;
        }

        public async Task<IEnumerable<ServiceTypeMaxAppointments?>> GetMaxAppointmentsDateByServiceTypeAsync()
        {
            var maxAppointmentsByServiceType = await _appointmentRepo.GetMaxAppointmentsDateByServiceTypeAsync();
            return maxAppointmentsByServiceType;
        }

        public async Task<Constants.MassageServices> GetMassageTypePreferenceAsync()
        {
            var massageTypePref = await _appointmentRepo.GetMassageTypePreferenceAsync();
            return massageTypePref;
        }

        public async Task<(DayOfWeek MaxDay, int MaxCount, DayOfWeek MinDay, int MinCount)> GetAppointmentsDayOfWeekReportAsync()
        {
            var maxAppointmentsTask = _appointmentRepo.GetMaxAppointmentsDayOfWeekAsync();
            var minAppointmentsTask = _appointmentRepo.GetMinAppointmentsDayOfWeekAsync();

            await Task.WhenAll(maxAppointmentsTask, minAppointmentsTask);

            var (maxDay, maxCount) = maxAppointmentsTask.Result;
            var (minDay, minCount) = minAppointmentsTask.Result;

            return (maxDay, maxCount, minDay, minCount);
        }

    }
}
