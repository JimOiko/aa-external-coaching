using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Appointment_Management_System.Models;

namespace Appointment_Management_System
{
    internal class Utilities
    {
        //#region Customers
        public static void CreateCustomer(List<Customer> customers)
        {
            Console.WriteLine("Create Customer");
            Console.WriteLine("---------------");

            Console.Write("Enter Name:");
            string name = Console.ReadLine();
            string email;
            while (true)
            {
                Console.Write("Enter Email: ");
                email = Console.ReadLine();
                if (IsValidEmail(email))
                {
                    break;
                }
                Console.WriteLine("Invalid email format. Please try again.");
            }
            string phoneNumber;
            while (true)
            {
                Console.Write("Enter Phone Number: ");
                phoneNumber = Console.ReadLine();
                if (IsValidPhoneNumber(phoneNumber))
                {
                    break;
                }
                Console.WriteLine("Invalid phone number. Please try again.");
            }

            customers.Add(new Customer(name, email, phoneNumber));
            Console.WriteLine("Customer created successfully.");
        }

        public static void ReadCustomers(List<Customer> customers)
        {
            Console.WriteLine("Customer List");
            Console.WriteLine("-------------");
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
            }
            else
            {
                Console.WriteLine("{0,-20} {1,-30} {2,-15}", "Name", "Email", "Phone Number");
                Console.WriteLine(new string('-', 65));
                foreach (var customer in customers)
                {
                    Console.WriteLine("{0,-20} {1,-30} {2,-15}", customer.Name, customer.Email, customer.PhoneNumber);
                }
            }
        }

        public static void UpdateCustomer(List<Customer> customers)
        {
            Console.WriteLine("Update Customer");
            Console.WriteLine("---------------");
            Console.Write("Enter the email of the customer to update: ");
            string email = Console.ReadLine();

            Customer customer = customers.Find(c => c.Email.ToLower() == email.ToLower());
            if (customer != null)
            {
                Console.Write("Enter new Name: ");
                customer.Name = Console.ReadLine();
                Console.Write("Enter new Email: ");
                customer.Email = Console.ReadLine();
                Console.Write("Enter new Phone Number: ");
                customer.PhoneNumber = Console.ReadLine();

                Console.WriteLine("Customer updated successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        public static void DeleteCustomer(List<Customer> customers)
        {
            Console.WriteLine("Delete Customer");
            Console.WriteLine("---------------");

            Console.Write("Enter the email of the customer to delete: ");
            string email = Console.ReadLine();

            Customer customer = customers.Find(c => c.Email.ToLower() == email.ToLower());
            if (customer != null)
            {
                customers.Remove(customer);
                Console.WriteLine("Customer deleted successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
        //#endregion

        //#region Appointments
        public static void CreateAppointment(List<Customer> customers, List<Appointment> appointments)
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

        public static void ViewAppointments(List<Appointment> appointments)
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

        public static void UpdateAppointment(List<Appointment> appointments)
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

        public static void DeleteAppointment(List<Appointment> appointments)
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
        //#endregion

        static bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        static bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length >= 10 && phoneNumber.Length <= 15 && long.TryParse(phoneNumber, out _);
        }
    }
}
