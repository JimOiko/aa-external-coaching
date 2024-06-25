// See https://aka.ms/new-console-template for more information
using Appointment_Management_System;
using Appointment_Management_System.Models;

List<Customer> customers = new List<Customer>();
List<Appointment> appointments = new List<Appointment>();
Console.WriteLine("Appointment Management System");
Console.WriteLine("==========================");
while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Create Customer");
    Console.WriteLine("2. View Customers");
    Console.WriteLine("3. Update Customer");
    Console.WriteLine("4. Delete Customer");
    Console.WriteLine("5. Create Appointment");
    Console.WriteLine("6. View Appointments");
    Console.WriteLine("7. Update Appointment");
    Console.WriteLine("8. Delete Appointment");
    Console.WriteLine("9. Exit");
    Console.Write("Select an option (1-9): ");

    string choice = Console.ReadLine();
    Console.WriteLine();
    switch (choice)
    {
        case "1":
            Utilities.CreateCustomer(customers);
            break;
        case "2":
            Utilities.ReadCustomers(customers);
            break;
        case "3":
            Utilities.UpdateCustomer(customers);
            break;
        case "4":
            Utilities.DeleteCustomer(customers);
            break;
        case "5":
            Utilities.CreateAppointment(customers,appointments);
            break;
        case "6":
            Utilities.ViewAppointments(appointments);
            break;
        case "7":
            Utilities.UpdateAppointment(appointments);
            break;
        case "8":
            Utilities.DeleteAppointment(appointments);
            break;
        case "9":
            Console.WriteLine("Exiting... Press any key to close.");
            Console.ReadKey();
            return;
        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }
}