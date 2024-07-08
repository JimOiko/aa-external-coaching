using AppointmentManagementSystem.Interfaces;
using AppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Services
{
    public class AppointmentDataEntryService: IDataEntryService
    {
        private readonly IManagementRepository<Appointment> _appointmentRepo;
        private readonly IManagementRepository<Customer> _customerRepo;

        public AppointmentDataEntryService(IManagementRepository<Appointment> appointmentRepo, IManagementRepository<Customer> customerRepo)
        {
            _appointmentRepo = appointmentRepo;
            _customerRepo = customerRepo;
        }

        public void Create()
        {
            Console.WriteLine("Create Appointment");
            Console.WriteLine("------------------");

            Console.Write("Enter the email of the customer: ");
            string customerEmail = Console.ReadLine() ?? "";

            Customer? customer = _customerRepo.GetById(customerEmail);
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
            ServiceType serviceType = (ServiceType)(serviceTypeChoice - 1);

            Console.Write("Enter date (yyyy-mm-dd): ");
            DateTime date;
            if (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            Console.Write("Enter time (HH:mm): ");
            string time = Console.ReadLine() ?? "";

            Console.Write("Enter optional notes: ");
            string? notes = Console.ReadLine() ?? "";

            if (serviceType == ServiceType.Massage)
            {
                CreateMassageAppointment(customer, date, time, notes);
            }
            else if (serviceType == ServiceType.PersonalTraining)
            {
                CreatePersonalTrainingAppointment(customer, date, time, notes);
            }
        }

        private void CreateMassageAppointment(Customer customer, DateTime date, string time, string notes)
        {
            Console.WriteLine("Massage Services: 1. Relaxing Massage, 2. Hot Stone Therapy, 3. Reflexology");
            Console.Write("Enter massage service type (1-3): ");
            int massageServiceChoice;
            if (!int.TryParse(Console.ReadLine(), out massageServiceChoice) || massageServiceChoice < 1 || massageServiceChoice > 3)
            {
                Console.WriteLine("Invalid massage service type.");
                return;
            }
            MassageServices massageServices = (MassageServices)(massageServiceChoice - 1);

            Console.WriteLine("Masseuse Preference: 1. Male, 2. Female");
            Console.Write("Enter massage Masseuse Preference (1-2): ");
            int masseusePreferenceChoice;
            if (!int.TryParse(Console.ReadLine(), out masseusePreferenceChoice) || masseusePreferenceChoice < 1 || masseusePreferenceChoice > 2)
            {
                Console.WriteLine("Invalid preference.");
                return;
            }

            MasseusePreference masseusePreference = (MasseusePreference)(masseusePreferenceChoice - 1);

            _appointmentRepo.Add(new MassageAppointment(customer, ServiceType.Massage, date, time, notes, massageServices, masseusePreference));
            Console.WriteLine("Massage appointment created successfully.");
        }

        private void CreatePersonalTrainingAppointment(Customer customer, DateTime date, string time, string notes)
        {
            Console.WriteLine("Training Duration: 1. 30 minutes, 2. 1 hour, 3. 1 hour and 30 minutes");
            Console.Write("Enter training duration (1-3): ");
            int trainingDurationChoice;
            if (!int.TryParse(Console.ReadLine(), out trainingDurationChoice) || trainingDurationChoice < 1 || trainingDurationChoice > 3)
            {
                Console.WriteLine("Invalid training duration.");
                return;
            }
            TrainingDuration trainingDuration = (TrainingDuration)(trainingDurationChoice - 1);

            Console.Write("Enter customer comments: ");
            string customerComments = Console.ReadLine() ?? "";

            Console.Write("Enter any injuries or pains: ");
            string injuriesOrPains = Console.ReadLine() ?? "";

            _appointmentRepo.Add(new PersonalTrainingAppointment(customer, ServiceType.PersonalTraining, date, time, notes, trainingDuration, customerComments, injuriesOrPains));
            Console.WriteLine("Personal training appointment created successfully.");
        }

        public void Read()
        {
            Console.WriteLine("Appointment List");
            Console.WriteLine("----------------");
            var appointments = _appointmentRepo.Get();

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
                    if (appointment is MassageAppointment massageAppointment)
                    {
                        Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-12} {4,-5} {5,-20} {6,-20} {7,-20} {8,-20} {9,-30} {10,-30}",
                                          massageAppointment.Id, massageAppointment.Customer.Name, massageAppointment.ServiceType,
                                          massageAppointment.Date.ToShortDateString(), massageAppointment.Time, massageAppointment.Notes,
                                          massageAppointment.MassageServices, massageAppointment.Preference, "N/A", "N/A", "N/A");
                    }
                    else if (appointment is PersonalTrainingAppointment trainingAppointment)
                    {
                        Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-12} {4,-5} {5,-20} {6,-20} {7,-20} {8,-20} {9,-30} {10,-30}",
                                          trainingAppointment.Id, trainingAppointment.Customer.Name, trainingAppointment.ServiceType,
                                          trainingAppointment.Date.ToShortDateString(), trainingAppointment.Time, trainingAppointment.Notes,
                                          "N/A", "N/A", trainingAppointment.TrainingDuration, trainingAppointment.CustomerComments,
                                          trainingAppointment.InjuriesOrPains);
                    }
                }
            }
        }

        public void Update()
        {
            Console.WriteLine("Update Appointment");
            Console.WriteLine("------------------");

            Console.Write("Enter the appointment ID to update: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var appointment = _appointmentRepo.GetById(id.ToString());
            if (appointment == null)
            {
                Console.WriteLine("Appointment not found.");
                return;
            }

            Console.WriteLine("Service Types: 1. Massage, 2. Personal Training");
            Console.Write("Enter new service type (leave blank to keep current): ");
            string serviceTypeInput = Console.ReadLine() ?? "";
            ServiceType? newServiceType = null;

            if (!string.IsNullOrWhiteSpace(serviceTypeInput))
            {
                if (int.TryParse(serviceTypeInput, out int serviceTypeChoice) && (serviceTypeChoice == 1 || serviceTypeChoice == 2))
                {
                    newServiceType = (ServiceType)(serviceTypeChoice - 1);
                }
                else
                {
                    Console.WriteLine("Invalid service type.");
                    return;
                }
            }

            UpdateCommonFields(appointment);
            if ((string.IsNullOrWhiteSpace(serviceTypeInput) && appointment.ServiceType == ServiceType.Massage) 
                || (newServiceType.HasValue && newServiceType.Value == ServiceType.Massage))
            {
                MassageAppointment? newAppointment = null;

                Console.WriteLine("Massage Services: 1. Relaxing Massage, 2. Hot Stone Therapy, 3. Reflexology");
                Console.Write("Enter new massage service: ");
                string massageServiceInput = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(massageServiceInput) || (!int.TryParse(massageServiceInput, out int massageServiceChoice)
                       || (massageServiceChoice != (int)MassageServices.RelaxingMassage + 1
                       && massageServiceChoice != (int)MassageServices.HotStoneTherapy + 1
                       && massageServiceChoice != (int)MassageServices.Reflexology + 1)))
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
                                appointment.Customer,
                            ServiceType.Massage,
                            appointment.Date,
                            appointment.Time,
                            appointment.Notes,
                            (MassageServices)(massageServiceChoice - 1),
                            (MasseusePreference)(preferenceChoice - 1)
                        );
                    _appointmentRepo.Add(newAppointment);
                    _appointmentRepo.Delete(appointment);
                    Console.WriteLine("Massage Appointment updated successfully.");
                }
                else
                {
                    var massageAppointment = (MassageAppointment)appointment;
                    massageAppointment.MassageServices = (MassageServices)(massageServiceChoice - 1);
                    massageAppointment.Preference = (MasseusePreference)(preferenceChoice - 1);
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
                    || (durationChoice != (int)TrainingDuration.ThirtyMinutes + 1
                    && durationChoice != (int)TrainingDuration.OneHour + 1
                    && durationChoice != (int)TrainingDuration.OneHourThirtyMinutes + 1)))
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
                                appointment.Customer,
                            ServiceType.PersonalTraining,
                            appointment.Date,
                            appointment.Time,
                            appointment.Notes,
                            (TrainingDuration)(durationChoice - 1),
                            comments,
                            injuriesOrPains
                        );
                    _appointmentRepo.Add(newAppointment);
                    _appointmentRepo.Delete(appointment);
                }
                else
                {
                    var trainingAppointment = (PersonalTrainingAppointment)appointment;
                    trainingAppointment.TrainingDuration = (TrainingDuration)(durationChoice - 1);
                    trainingAppointment.CustomerComments = string.IsNullOrWhiteSpace(comments) ? trainingAppointment.CustomerComments : comments;
                    trainingAppointment.InjuriesOrPains = string.IsNullOrWhiteSpace(injuriesOrPains) ? trainingAppointment.InjuriesOrPains: injuriesOrPains; ;
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

        public void Delete()
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

            var appointment = _appointmentRepo.GetById(id.ToString());
            if (appointment != null)
            {
                _appointmentRepo.Delete(appointment);
                Console.WriteLine("Appointment deleted successfully.");
            }
            else
            {
                Console.WriteLine("Appointment not found.");
            }
        }
    }
}
