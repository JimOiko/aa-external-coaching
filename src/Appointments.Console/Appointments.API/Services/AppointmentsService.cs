using AppointmentManagementSystem.DomainObjects;
using Appointments.API.Interfaces;
using Appointments.DAL.Interfaces;

namespace Appointments.API.Services
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly IAppointmentRepository _appointmentRepo;

        public AppointmentsService(IAppointmentRepository appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepo.GetAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId)
        {
            return await _appointmentRepo.GetByIdAsync(appointmentId.ToString());
        }

        public async Task CreateAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepo.AddAsync(appointment);
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepo.UpdateAsync(appointment);
        }

        public async Task DeleteAppointmentAsync(Guid appointmentId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId.ToString());
            if (appointment != null)
            {
                await _appointmentRepo.DeleteAsync(appointment);
            }
        }
    }
}
