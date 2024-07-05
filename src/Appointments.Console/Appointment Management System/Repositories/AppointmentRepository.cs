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

            appointments.Add(new Appointment(customer, serviceType, date, time, notes));
            Console.WriteLine("Appointment created successfully.");
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
                    Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-12} {4,-5} {5,-20}", appointment.Id, appointment.Customer.Name, appointment.ServiceType, appointment.Date.ToShortDateString(), appointment.Time, appointment.Notes);
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
