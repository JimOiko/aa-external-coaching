using AppointmentManagementSystem.DomainObjects;
using Appointments.BLL.Interfaces;

namespace AppointmentManagementSystem.Services
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;

    public partial class AppointmentDataEntryService : IAppointmentDataEntryService
    {
        public async Task UpdateAsync()
        {
            Console.WriteLine("Update Appointment");
            Console.WriteLine("------------------");

            Console.Write("Enter the appointment ID to update: ");
            Guid id;
            if (!Guid.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var appointment = await _appointmentRepo.GetByIdAsync(id.ToString());
            if (appointment == null)
            {
                Console.WriteLine("Appointment not found.");
                return;
            }

            Console.WriteLine("Service Types: 1. Massage, 2. Personal Training");
            Console.Write("Enter new service type (leave blank to keep current): ");
            string serviceTypeInput = Console.ReadLine() ?? "";
            AllEnums.ServiceType? newServiceType = null;

            if (!string.IsNullOrWhiteSpace(serviceTypeInput))
            {
                if (int.TryParse(serviceTypeInput, out int serviceTypeChoice) && (serviceTypeChoice == 1 || serviceTypeChoice == 2))
                {
                    newServiceType = (AllEnums.ServiceType)(serviceTypeChoice - 1);
                }
                else
                {
                    Console.WriteLine("Invalid service type.");
                    return;
                }
            }

            UpdateCommonFields(appointment);
            if ((string.IsNullOrWhiteSpace(serviceTypeInput) && appointment.ServiceType == AllEnums.ServiceType.Massage)
                || (newServiceType.HasValue && newServiceType.Value == AllEnums.ServiceType.Massage))
            {
                MassageAppointment? newAppointment = null;

                Console.WriteLine("Massage Services: 1. Relaxing Massage, 2. Hot Stone Therapy, 3. Reflexology");
                Console.Write("Enter new massage service: ");
                string massageServiceInput = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(massageServiceInput) || (!int.TryParse(massageServiceInput, out int massageServiceChoice)
                       || (massageServiceChoice != (int)AllEnums.MassageServices.RelaxingMassage + 1
                       && massageServiceChoice != (int)AllEnums.MassageServices.HotStoneTherapy + 1
                       && massageServiceChoice != (int)AllEnums.MassageServices.Reflexology + 1)))
                {
                    Console.WriteLine("Invalid massage service.");
                    return;
                }

                Console.Write("Enter new masseuse preference (1 for Male, 2 for Female): ");
                string preferenceInput = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(preferenceInput) || (!int.TryParse(preferenceInput, out int preferenceChoice) || (preferenceChoice != 1 && preferenceChoice != 2)))
                {
                    Console.WriteLine("Invalid masseuse preference.");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(serviceTypeInput) && newServiceType != appointment.ServiceType)
                {
                    newAppointment = new MassageAppointment(
                                appointment.CustomerId,
                            AllEnums.ServiceType.Massage,
                            appointment.Date,
                            appointment?.Time,
                            appointment?.Notes,
                            (AllEnums.MassageServices)(massageServiceChoice - 1),
                            (AllEnums.MasseusePreference)(preferenceChoice - 1)
                        );
                    await _appointmentRepo.AddAsync(newAppointment);
                    if (appointment != null)
                        await _appointmentRepo.DeleteAsync(appointment);
                    Console.WriteLine("Massage Appointment updated successfully.");
                }
                else
                {
                    var massageAppointment = (MassageAppointment)appointment;
                    massageAppointment.MassageServices = (AllEnums.MassageServices)(massageServiceChoice - 1);
                    massageAppointment.Preference = (AllEnums.MasseusePreference)(preferenceChoice - 1);
                    await _appointmentRepo.UpdateAsync(massageAppointment);
                }
            }
            else
            {
                PersonalTrainingAppointment? newAppointment = null;

                Console.WriteLine("Training Duration: 1. 30 minutes, 2. 1 hour, 3. 1 hour and 30 minutes");
                Console.Write("Enter new training duration: ");
                var durationInput = Console.ReadLine() ?? "";
                var durationChoice = 0;
                if (string.IsNullOrWhiteSpace(durationInput) ||
                    (!int.TryParse(durationInput, out durationChoice)
                    || (durationChoice != (int)AllEnums.TrainingDuration.ThirtyMinutes + 1
                    && durationChoice != (int)AllEnums.TrainingDuration.OneHour + 1
                    && durationChoice != (int)AllEnums.TrainingDuration.OneHourThirtyMinutes + 1)))
                {
                    Console.WriteLine("Invalid training duration.");
                    return;
                }

                Console.Write("Enter new customer comments (leave blank to keep current): ");
                string comments = Console.ReadLine() ?? "";

                Console.Write("Enter new injuries or pains (leave blank to keep current): ");
                string injuriesOrPains = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(serviceTypeInput) && newServiceType != appointment.ServiceType)
                {
                    newAppointment = new PersonalTrainingAppointment(
                                appointment.CustomerId,
                            AllEnums.ServiceType.PersonalTraining,
                            appointment.Date,
                            appointment?.Time,
                            appointment?.Notes,
                            (AllEnums.TrainingDuration)(durationChoice - 1),
                            comments,
                            injuriesOrPains
                        );
                    await _appointmentRepo.AddAsync(newAppointment);

                    if (appointment != null)
                        await _appointmentRepo.DeleteAsync(appointment);
                }
                else
                {
                    var trainingAppointment = (PersonalTrainingAppointment)appointment;
                    trainingAppointment.TrainingDuration = (AllEnums.TrainingDuration)(durationChoice - 1);
                    trainingAppointment.CustomerComments = string.IsNullOrWhiteSpace(comments) ? trainingAppointment.CustomerComments : comments;
                    trainingAppointment.InjuriesOrPains = string.IsNullOrWhiteSpace(injuriesOrPains) ? trainingAppointment.InjuriesOrPains : injuriesOrPains; ;
                    await _appointmentRepo.UpdateAsync(trainingAppointment);
                }
            }
            Console.WriteLine("Appointment updated successfully.");
        }

        private void UpdateCommonFields(Appointment appointment)
        {
            Console.Write("Enter new date (yyyy-mm-dd) (leave blank to keep current): ");
            string dateInput = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(dateInput) && DateTime.TryParse(dateInput, out DateTime date))
            {
                appointment.Date = date;
            }

            Console.Write("Enter new time (HH:mm) (leave blank to keep current): ");
            string timeInput = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(timeInput))
            {
                appointment.Time = timeInput;
            }

            Console.Write("Enter new optional notes (leave blank to keep current): ");
            string notesInput = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(notesInput))
            {
                appointment.Notes = notesInput;
            }
        }
    }
}

