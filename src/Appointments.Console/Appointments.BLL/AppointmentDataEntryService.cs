using AppointmentManagementSystem.DomainObjects;
using Appointments.BLL;
using Appointments.BLL.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace AppointmentManagementSystem.Services
{
    using static System.Net.WebRequestMethods;
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public partial class AppointmentDataEntryService(HttpClient httpClient, IDiscountService discountService, IOptions<ApiSettings> apiSettings) : IAppointmentDataEntryService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IDiscountService discountService = discountService;
        // Base URLs for the APIs
        private readonly string _customersApiUrl = apiSettings.Value.CustomerApiUrl;
        private readonly string _appointmentsApiUrl = apiSettings.Value.AppointmentApiUrl;

        public async Task CreateAsync()
        {
            Console.WriteLine("Create Appointment");
            Console.WriteLine("------------------");

            Console.Write("Enter the email of the customer: ");
            string customerEmail = Console.ReadLine() ?? "";

            var response = await _httpClient.GetAsync($"{_customersApiUrl}/getByEmail/{customerEmail}");
            response.EnsureSuccessStatusCode();
            var customer = await response.Content.ReadFromJsonAsync<Customer>();
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            Console.WriteLine("Service Types: 1. Massage, 2. Personal Training");
            Console.Write("Enter service type (1 or 2): ");
            int serviceTypeChoice;
            if (!int.TryParse(Console.ReadLine(), out serviceTypeChoice) || serviceTypeChoice != 1 && serviceTypeChoice != 2)
            {
                Console.WriteLine("Invalid service type.");
                return;
            }
            var serviceType = (AllEnums.ServiceType)(serviceTypeChoice - 1);

            Console.Write("Enter date (yyyy-mm-dd): ");
            DateTimeOffset date;
            if (!DateTimeOffset.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            Console.Write("Enter time (HH:mm): ");
            string time = Console.ReadLine() ?? "";

            Console.Write("Enter optional notes: ");
            string? notes = Console.ReadLine() ?? "";

            if (serviceType == AllEnums.ServiceType.Massage)
            {
                await CreateMassageAppointment(customer.Id, date, time, notes);
            }
            else if (serviceType == AllEnums.ServiceType.PersonalTraining)
            {
               await CreatePersonalTrainingAppointment(customer.Id, date, time, notes);
            }
            try
            {
                var isNameday = await discountService.ProcessDiscountAsync(customer.Id, date);
                if (isNameday)
                {
                    // Apply discount
                    Console.WriteLine($"Applying discount for {customer.Name} because the appointment is on their nameday!");
                }
                else
                {
                    Console.WriteLine($"No discount applicable. The appointment on {date.Date.ToShortDateString()} is not on {customer.Name}'s nameday.");
                }
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific InvalidOperationException
                Console.WriteLine("Operation failed: " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                // Handle specific HttpRequestException from nameday API
                Console.WriteLine("Error with the nameday API: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Catch all other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private async Task CreateMassageAppointment(Guid customerId, DateTimeOffset date, string time, string notes)
        {
            Console.WriteLine("Massage Services: 1. Relaxing Massage, 2. Hot Stone Therapy, 3. Reflexology");
            Console.Write("Enter massage service type (1-3): ");
            int massageServiceChoice;
            if (!int.TryParse(Console.ReadLine(), out massageServiceChoice) || massageServiceChoice < 1 || massageServiceChoice > 3)
            {
                Console.WriteLine("Invalid massage service type.");
                return;
            }
            AllEnums.MassageServices massageServices = (AllEnums.MassageServices)(massageServiceChoice - 1);

            Console.WriteLine("Masseuse Preference: 1. Male, 2. Female");
            Console.Write("Enter massage Masseuse Preference (1-2): ");
            int masseusePreferenceChoice;
            if (!int.TryParse(Console.ReadLine(), out masseusePreferenceChoice) || masseusePreferenceChoice < 1 || masseusePreferenceChoice > 2)
            {
                Console.WriteLine("Invalid preference.");
                return;
            }

            AllEnums.MasseusePreference masseusePreference = (AllEnums.MasseusePreference)(masseusePreferenceChoice - 1);

            var appointment = new MassageAppointment(customerId, AllEnums.ServiceType.Massage, date, time, notes, massageServices, masseusePreference);
            var response = await _httpClient.PostAsJsonAsync($"{_appointmentsApiUrl}/create", appointment);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Massage appointment created successfully.");
        }

        private async Task CreatePersonalTrainingAppointment(Guid customerId, DateTimeOffset date, string time, string notes)
        {
            Console.WriteLine("Training Duration: 1. 30 minutes, 2. 1 hour, 3. 1 hour and 30 minutes");
            Console.Write("Enter training duration (1-3): ");
            int trainingDurationChoice;
            if (!int.TryParse(Console.ReadLine(), out trainingDurationChoice) || trainingDurationChoice < 1 || trainingDurationChoice > 3)
            {
                Console.WriteLine("Invalid training duration.");
                return;
            }
            AllEnums.TrainingDuration trainingDuration = (AllEnums.TrainingDuration)(trainingDurationChoice - 1);

            Console.Write("Enter customer comments: ");
            string customerComments = Console.ReadLine() ?? "";

            Console.Write("Enter any injuries or pains: ");
            string injuriesOrPains = Console.ReadLine() ?? "";

            var appointment = new PersonalTrainingAppointment(customerId, AllEnums.ServiceType.PersonalTraining, date, time, notes, trainingDuration, customerComments, injuriesOrPains);
            var response = await _httpClient.PostAsJsonAsync($"{_appointmentsApiUrl}/create", appointment);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Personal training appointment created successfully.");
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

            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments found.");
            }
            else
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-12} {4,-5} {5,-20} {6,-20} {7,-20} {8,-20} {9,-30} {10,-30}",
                          "ID", "Customer", "Service", "Date", "Time", "Notes", "Massage Service", "Masseuse Preference",
                          "Training Duration", "Customer Comments", "Injuries/Pains");
                Console.WriteLine(new string('-', 220));
                foreach (var appointment in appointments)
                {
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
            if (!Guid.TryParse(Console.ReadLine(), out id))
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
