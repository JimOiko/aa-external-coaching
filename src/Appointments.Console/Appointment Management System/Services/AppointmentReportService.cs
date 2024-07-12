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
    }
}
