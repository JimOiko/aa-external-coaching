using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Infastructure.Interfaces;
using AppointmentManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Services
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public partial class AppointmentDataEntryService(IAppointmentRepository appointmentRepo, ICustomerRepository customerRepo, IDiscountService discountService) : IAppointmentDataEntryService
    {
        private readonly IAppointmentRepository _appointmentRepo = appointmentRepo;
        private readonly ICustomerRepository _customerRepo = customerRepo;
        private readonly IDiscountService discountService = discountService;

        public async Task CreateAsync()
        {
            Console.WriteLine("Create Appointment");
            Console.WriteLine("------------------");

            Console.Write("Enter the email of the customer: ");
            string customerEmail = Console.ReadLine() ?? "";

            Customer? customer = await _customerRepo.GetByEmailAsync(customerEmail);
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
                CreateMassageAppointment(customer.Id, date, time, notes);
            }
            else if (serviceType == AllEnums.ServiceType.PersonalTraining)
            {
                CreatePersonalTrainingAppointment(customer.Id, date, time, notes);
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
            catch (NullReferenceException)
            {
                Console.WriteLine($"Customer Does Not Exist!");
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

            await _appointmentRepo.AddAsync(new MassageAppointment(customerId, AllEnums.ServiceType.Massage, date, time, notes, massageServices, masseusePreference));
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

            await _appointmentRepo.AddAsync(new PersonalTrainingAppointment(customerId, AllEnums.ServiceType.PersonalTraining, date, time, notes, trainingDuration, customerComments, injuriesOrPains));
            Console.WriteLine("Personal training appointment created successfully.");
        }

        public async Task ReadAsync()
        {
            Console.WriteLine("Appointment List");
            Console.WriteLine("----------------");
            var appointments = await _appointmentRepo.GetAsync();

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
                    var customer = await _customerRepo.GetByIdAsync(appointment.CustomerId);
                    if (appointment is MassageAppointment massageAppointment)
                    {
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
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var appointment = await _appointmentRepo.GetByIdAsync(id.ToString());
            if (appointment != null)
            {
                await _appointmentRepo.DeleteAsync(appointment);
                Console.WriteLine("Appointment deleted successfully.");
            }
            else
            {
                Console.WriteLine("Appointment not found.");
            }
        }

    }
}
