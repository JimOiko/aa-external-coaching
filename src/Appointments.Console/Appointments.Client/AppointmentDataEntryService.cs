using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.DomainObjects.Interfaces;
using AppointmentManagementSystem.Abstractions;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using Appointments.BLL;

namespace Appointments.Client
{
    using static System.Net.WebRequestMethods;
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public partial class AppointmentDataEntryService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, IUserInputService userInputService) : IAppointmentDataEntryService
    {
        private readonly HttpClient _httpClient = httpClient;
        // Base URLs for the APIs
        private readonly string _customersApiUrl = apiSettings.Value.CustomerApiUrl;
        private readonly string _appointmentsApiUrl = apiSettings.Value.AppointmentApiUrl;
        private readonly IUserInputService _userInputService = userInputService;

        public async Task<Appointment?> CreateAsync()
        {
            Console.WriteLine("Create Appointment");
            Console.WriteLine("------------------");

            Console.Write("Enter the email of the customer: ");
            string customerEmail = _userInputService.ReadLine();

            var response = await _httpClient.GetAsync($"{_customersApiUrl}/getByEmail/{customerEmail}");
            response.EnsureSuccessStatusCode();
            var customer = await response.Content.ReadFromJsonAsync<Customer>();
            if (customer == null || string.IsNullOrEmpty(customer.Email))
            {
                Console.WriteLine("Customer not found.");
                return null;
            }

            Console.WriteLine("Service Types: 1. Massage, 2. Personal Training");
            Console.Write("Enter service type (1 or 2): ");
            int serviceTypeChoice;
            if (!int.TryParse(_userInputService.ReadLine(), out serviceTypeChoice) || serviceTypeChoice != 1 && serviceTypeChoice != 2)
            {
                Console.WriteLine("Invalid service type.");
                return null;
            }
            var serviceType = (AllEnums.ServiceType)(serviceTypeChoice - 1);

            Console.Write("Enter date (yyyy-mm-dd): ");
            DateTimeOffset date;
            if (!DateTimeOffset.TryParse(_userInputService.ReadLine(), out date))
            {
                Console.WriteLine("Invalid date format.");
                return null;
            }

            Console.Write("Enter time (HH:mm): ");
            string time = _userInputService.ReadLine();

            Console.Write("Enter optional notes: ");
            string? notes = _userInputService.ReadLine();

            Appointment? appointment = null;
            if (serviceType == AllEnums.ServiceType.Massage)
            {
                appointment = await CreateMassageAppointment(customer.Id, date, time, notes);
            }
            else if (serviceType == AllEnums.ServiceType.PersonalTraining)
            {
                appointment = await CreatePersonalTrainingAppointment(customer.Id, date, time, notes);
            }

            if (appointment != null)
            {
                // Make the request to create the appointment
                var createResponse = await _httpClient.PostAsJsonAsync($"{_appointmentsApiUrl}/create", appointment);

                if (createResponse.IsSuccessStatusCode)
                {
                    // Parse the response content for the isNameday flag using JsonDocument
                    var responseContent = await createResponse.Content.ReadAsStringAsync();
                    using (JsonDocument jsonDocument = JsonDocument.Parse(responseContent))
                    {
                        JsonElement root = jsonDocument.RootElement;

                        if (root.TryGetProperty("isNameday", out JsonElement isNamedayElement))
                        {
                            bool isNameday = isNamedayElement.GetBoolean();

                            if (isNameday)
                            {
                                Console.WriteLine($"A discount has been applied because the appointment is on {customer.Name}'s nameday.");
                            }
                            else
                            {
                                Console.WriteLine("Appointment created successfully. No discount applicable.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Response does not contain isNameday flag.");
                        }
                    }
                }
                else
                {
                    // Handle non-successful response
                    Console.WriteLine($"Failed to create the appointment. Status code: {createResponse.StatusCode}");
                    var errorMessage = await createResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorMessage}");
                }
            }

            return appointment;
        }

        private async Task<MassageAppointment?> CreateMassageAppointment(Guid customerId, DateTimeOffset date, string time, string notes)
        {
            Console.WriteLine("Massage Services: 1. Relaxing Massage, 2. Hot Stone Therapy, 3. Reflexology");
            Console.Write("Enter massage service type (1-3): ");
            int massageServiceChoice;
            if (!int.TryParse(_userInputService.ReadLine(), out massageServiceChoice) || massageServiceChoice < 1 || massageServiceChoice > 3)
            {
                Console.WriteLine("Invalid massage service type.");
                return null;
            }
            AllEnums.MassageServices massageServices = (AllEnums.MassageServices)(massageServiceChoice - 1);

            Console.WriteLine("Masseuse Preference: 1. Male, 2. Female");
            Console.Write("Enter massage Masseuse Preference (1-2): ");
            int masseusePreferenceChoice;
            if (!int.TryParse(_userInputService.ReadLine(), out masseusePreferenceChoice) || masseusePreferenceChoice < 1 || masseusePreferenceChoice > 2)
            {
                Console.WriteLine("Invalid preference.");
                return null;
            }

            AllEnums.MasseusePreference masseusePreference = (AllEnums.MasseusePreference)(masseusePreferenceChoice - 1);

            var appointment = new MassageAppointment(customerId, AllEnums.ServiceType.Massage, date, time, notes, massageServices, masseusePreference);

            // Note: The actual POST request is now done in the CreateAsync method
            return appointment;
        }

        private async Task<PersonalTrainingAppointment?> CreatePersonalTrainingAppointment(Guid customerId, DateTimeOffset date, string time, string notes)
        {
            Console.WriteLine("Training Duration: 1. 30 minutes, 2. 1 hour, 3. 1 hour and 30 minutes");
            Console.Write("Enter training duration (1-3): ");
            int trainingDurationChoice;
            if (!int.TryParse(_userInputService.ReadLine(), out trainingDurationChoice) || trainingDurationChoice < 1 || trainingDurationChoice > 3)
            {
                Console.WriteLine("Invalid training duration.");
                return null;
            }
            AllEnums.TrainingDuration trainingDuration = (AllEnums.TrainingDuration)(trainingDurationChoice - 1);

            Console.Write("Enter customer comments: ");
            string customerComments = _userInputService.ReadLine();

            Console.Write("Enter any injuries or pains: ");
            string injuriesOrPains = _userInputService.ReadLine();

            var appointment = new PersonalTrainingAppointment(customerId, AllEnums.ServiceType.PersonalTraining, date, time, notes, trainingDuration, customerComments, injuriesOrPains);

            // Note: The actual POST request is now done in the CreateAsync method
            return appointment;
        }

        public async Task ReadAsync()
        {
            Console.WriteLine("Appointment List");
            Console.WriteLine("----------------");
            var response = await _httpClient.GetAsync($"{_appointmentsApiUrl}/get");
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
            var appointments = System.Text.Json.JsonSerializer.Deserialize<List<Appointment>>(jsonResponse, options);

            if (appointments?.Count == 0)
            {
                Console.WriteLine("No appointments found.");
            }
            else
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-12} {4,-5} {5,-20} {6,-20} {7,-20} {8,-20} {9,-30} {10,-30}",
                          "ID", "Customer", "Service", "Date", "Time", "Notes", "Massage Service", "Masseuse Preference",
                          "Training Duration", "Customer Comments", "Injuries/Pains");
                Console.WriteLine(new string('-', 220));
                for (int i = 0; i < appointments?.Count; i++)
                {
                    Appointment? appointment = appointments[i];
                    var customerResponse = await _httpClient.GetAsync($"{_customersApiUrl}/getById/{appointment.CustomerId}");
                    customerResponse.EnsureSuccessStatusCode();
                    var customer = await customerResponse.Content.ReadFromJsonAsync<Customer>();
                    if (appointment.ServiceType == AllEnums.ServiceType.Massage )
                    {
                        MassageAppointment massageAppointment = (MassageAppointment)appointment;
                        Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-12} {4,-5} {5,-20} {6,-20} {7,-20} {8,-20} {9,-30} {10,-30}",
                                          massageAppointment.AppointmentId, customer?.Name, massageAppointment.ServiceType,
                                          massageAppointment.Date.DateTime.ToShortDateString(), massageAppointment.Time, massageAppointment.Notes,
                                          massageAppointment.MassageServices, massageAppointment.Preference, "N/A", "N/A", "N/A");
                    }
                    else if (appointment is PersonalTrainingAppointment trainingAppointment)
                    {
                        Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-12} {4,-5} {5,-20} {6,-20} {7,-20} {8,-20} {9,-30} {10,-30}",
                                          trainingAppointment.AppointmentId, customer?.Name, trainingAppointment.ServiceType,
                                          trainingAppointment.Date.DateTime.ToShortDateString(), trainingAppointment.Time, trainingAppointment.Notes,
                                          "N/A", "N/A", trainingAppointment.TrainingDuration, trainingAppointment.CustomerComments,
                                          trainingAppointment.InjuriesOrPains);
                    }
                }
            }
        }

        public async Task DeleteAsync()
        {
            Console.WriteLine("Delete Appointment");
            Console.WriteLine("------------------");

            Console.Write("Enter the appointment ID to delete: ");
            Guid id;
            if (!Guid.TryParse(_userInputService.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var response = await _httpClient.DeleteAsync($"{_appointmentsApiUrl}/delete/{id}");
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Appointment deleted successfully.");
        }


        private void CombineProperties(Appointment baseAppointment, Appointment specificAppointment)
        {
            specificAppointment.Date = baseAppointment.Date;
            specificAppointment.Time = baseAppointment.Time;
            specificAppointment.Notes = baseAppointment.Notes;
        }
    }
}
