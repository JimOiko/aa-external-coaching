using Appointment_Management_System.Interfaces;
using Appointment_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Management_System.Repositories
{
    public class AppointmentRepository: IManagementRepository<Appointment>
    {
        public void Create( List<Appointment> appointments, List<Customer> customers)
        {
            Console.WriteLine("Create Appointment");
            Console.WriteLine("------------------");

            Console.Write("Enter the email of the customer: ");
            string customerEmail = Console.ReadLine();

            Customer customer = customers.Find(c => c.Email.Equals(customerEmail, StringComparison.OrdinalIgnoreCase));
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
            string time = Console.ReadLine();

            Console.Write("Enter optional notes: ");
            string notes = Console.ReadLine();

            if (serviceType == ServiceType.Massage)
            {
                CreateMassageAppointment(appointments, customer, date, time, notes);
            }
            else if (serviceType == ServiceType.PersonalTraining)
            {
                CreatePersonalTrainingAppointment(appointments, customer, date, time, notes);
            }
        }

        private void CreateMassageAppointment(List<Appointment> appointments, Customer customer, DateTime date, string time, string notes)
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

            MasseusePreference masseusePreference = (MasseusePreference)(massageServiceChoice - 1);

            appointments.Add(new MassageAppointment(customer, ServiceType.Massage, date, time, notes, massageServices, masseusePreference));
            Console.WriteLine("Massage appointment created successfully.");
        }

        private void CreatePersonalTrainingAppointment(List<Appointment> appointments, Customer customer, DateTime date, string time, string notes)
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
            string customerComments = Console.ReadLine();

            Console.Write("Enter any injuries or pains: ");
            string injuriesOrPains = Console.ReadLine();

            appointments.Add(new PersonalTrainingAppointment(customer, ServiceType.PersonalTraining, date, time, notes, trainingDuration, customerComments, injuriesOrPains));
            Console.WriteLine("Personal training appointment created successfully.");
        }

        public void Read(List<Appointment> appointments)
        {
            Console.WriteLine("Appointment List");
            Console.WriteLine("----------------");

            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments found.");
            }
            else
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-12} {4,-5} {5,-20}", "ID", "Customer", "Service", "Date", "Time", "Notes");
                Console.WriteLine(new string('-', 80));
                foreach (var appointment in appointments)
                {
                    //   Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-12} {4,-5} {5,-20}", appointment.Id, appointment.Customer.Name, appointment.ServiceType, appointment.Date.ToShortDateString(), appointment.Time, appointment.Notes);
                    Console.WriteLine(appointment);
                }
            }
        }

        public void Update(List<Appointment> appointments)
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

            Appointment appointment = appointments.Find(a => a.Id == id);
            if (appointment == null)
            {
                Console.WriteLine("Appointment not found.");
                return;
            }

            Console.WriteLine("Service Types: 1. Massage, 2. Personal Training");
            Console.Write("Enter new service type (leave blank to keep current): ");
            string serviceTypeInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(serviceTypeInput))
            {
                int serviceTypeChoice;
                if (!int.TryParse(serviceTypeInput, out serviceTypeChoice) || serviceTypeChoice != 1 && serviceTypeChoice != 2)
                {
                    Console.WriteLine("Invalid service type.");
                    return;
                }
                appointment.ServiceType = (ServiceType)(serviceTypeChoice - 1);
            }

            Console.Write("Enter new date (yyyy-mm-dd) (leave blank to keep current): ");
            string dateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dateInput))
            {
                DateTime date;
                if (!DateTime.TryParse(dateInput, out date))
                {
                    Console.WriteLine("Invalid date format.");
                    return;
                }
                appointment.Date = date;
            }

            Console.Write("Enter new time (HH:mm) (leave blank to keep current): ");
            string time = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(time))
            {
                appointment.Time = time;
            }

            Console.Write("Enter new optional notes (leave blank to keep current): ");
            string notes = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(notes))
            {
                appointment.Notes = notes;
            }

            Console.WriteLine("Appointment updated successfully.");
        }

        public void Delete(List<Appointment> appointments)
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

            Appointment appointment = appointments.Find(a => a.Id == id);
            if (appointment != null)
            {
                appointments.Remove(appointment);
                Console.WriteLine("Appointment deleted successfully.");
            }
            else
            {
                Console.WriteLine("Appointment not found.");
            }
        }
    }
}
