using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Abstractions;
using System.Net.Http.Json;
using System.Text.Json;
using Appointments.BLL;

namespace Appointments.Client
{
    using Constants = AppointmentManagementSystem.DomainObjects.Enums;

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
            var response = await _httpClient.GetAsync($"{_appointmentsApiUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(jsonResponse);
            var rootElement = document.RootElement;

            //var appointments = new List<Appointment>();
            var options = new JsonSerializerOptions
            {
                Converters = { new AppointmentJsonConverter() },
                PropertyNameCaseInsensitive = true
            };
            var appointment = System.Text.Json.JsonSerializer.Deserialize<Appointment>(jsonResponse, options);
            if (appointment == null)
            {
                Console.WriteLine("Appointment not found.");
                return;
            }

            Console.WriteLine("Service Types: 1. Massage, 2. Personal Training");
            Console.Write("Enter new service type (leave blank to keep current): ");
            string serviceTypeInput = Console.ReadLine() ?? "";
            Constants.ServiceType? newServiceType = null;

            if (!string.IsNullOrWhiteSpace(serviceTypeInput))
            {
                if (int.TryParse(serviceTypeInput, out int serviceTypeChoice) && (serviceTypeChoice == 1 || serviceTypeChoice == 2))
                {
                    newServiceType = (Constants.ServiceType)(serviceTypeChoice - 1);
                }
                else
                {
                    Console.WriteLine("Invalid service type.");
                    return;
                }
            }

            UpdateCommonFields(appointment);
            if ((string.IsNullOrWhiteSpace(serviceTypeInput) && appointment.ServiceType == Constants.ServiceType.Massage)
                || (newServiceType.HasValue && newServiceType.Value == Constants.ServiceType.Massage))
            {
                MassageAppointment? newAppointment = null;

                Console.WriteLine("Massage Services: 1. Relaxing Massage, 2. Hot Stone Therapy, 3. Reflexology");
                Console.Write("Enter new massage service: ");
                string massageServiceInput = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(massageServiceInput) || (!int.TryParse(massageServiceInput, out int massageServiceChoice)
                       || (massageServiceChoice != (int)Constants.MassageServices.RelaxingMassage + 1
                       && massageServiceChoice != (int)Constants.MassageServices.HotStoneTherapy + 1
                       && massageServiceChoice != (int)Constants.MassageServices.Reflexology + 1)))
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
                            Constants.ServiceType.Massage,
                            appointment.Date,
                            appointment?.Time,
                            appointment?.Notes,
                            (Constants.MassageServices)(massageServiceChoice - 1),
                            (Constants.MasseusePreference)(preferenceChoice - 1)
                        );
                    var addResponse = await _httpClient.PostAsJsonAsync(_appointmentsApiUrl, newAppointment);
                    addResponse.EnsureSuccessStatusCode();
                    if (appointment != null)
                        await _httpClient.DeleteAsync($"{_appointmentsApiUrl}/{appointment.AppointmentId}");
                    Console.WriteLine("Massage Appointment updated successfully.");
                }
                else
                {
                    var massageAppointment = (MassageAppointment)appointment;
                    massageAppointment.MassageServices = (Constants.MassageServices)(massageServiceChoice - 1);
                    massageAppointment.Preference = (Constants.MasseusePreference)(preferenceChoice - 1);
                    var updateResponse = await _httpClient.PutAsJsonAsync($"{_appointmentsApiUrl}/{id}", massageAppointment);
                    updateResponse.EnsureSuccessStatusCode();
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
                    || (durationChoice != (int)Constants.TrainingDuration.ThirtyMinutes + 1
                    && durationChoice != (int)Constants.TrainingDuration.OneHour + 1
                    && durationChoice != (int)Constants.TrainingDuration.OneHourThirtyMinutes + 1)))
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
                            Constants.ServiceType.PersonalTraining,
                            appointment.Date,
                            appointment?.Time,
                            appointment?.Notes,
                            (Constants.TrainingDuration)(durationChoice - 1),
                            comments,
                            injuriesOrPains
                        );
                    var addResponse = await _httpClient.PostAsJsonAsync(_appointmentsApiUrl, newAppointment);
                    addResponse.EnsureSuccessStatusCode();
                    if (appointment != null)
                        await _httpClient.DeleteAsync($"{_appointmentsApiUrl}/delete/{id}");
                }
                else
                {
                    var trainingAppointment = (PersonalTrainingAppointment)appointment;
                    trainingAppointment.TrainingDuration = (Constants.TrainingDuration)(durationChoice - 1);
                    trainingAppointment.CustomerComments = string.IsNullOrWhiteSpace(comments) ? trainingAppointment.CustomerComments : comments;
                    trainingAppointment.InjuriesOrPains = string.IsNullOrWhiteSpace(injuriesOrPains) ? trainingAppointment.InjuriesOrPains : injuriesOrPains; ;
                    var updateResponse = await _httpClient.PutAsJsonAsync($"{_appointmentsApiUrl}/update/{id}", trainingAppointment);
                    updateResponse.EnsureSuccessStatusCode();
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

