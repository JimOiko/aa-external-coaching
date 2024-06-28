// See https://aka.ms/new-console-template for more information
using Appointment_Management_System;
using Appointment_Management_System.Interfaces;
using Appointment_Management_System.Models;
using Appointment_Management_System.Repositories;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
              .AddSingleton<IManagementRepository<Customer>, CustomerRepository>()
              .AddSingleton<IManagementRepository<Appointment>, AppointmentRepository>()
              .BuildServiceProvider();

var customerRepo = serviceProvider.GetService<IManagementRepository<Customer>>();
var appointmentRepo = serviceProvider.GetService<IManagementRepository<Appointment>>();

List<Customer> customers = [];
List<Appointment> appointments = [];
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
            customerRepo.Create(customers);
            break;
        case "2":
            customerRepo.Read(customers);
            break;
        case "3":
            customerRepo.Update(customers);
            break;
        case "4":
            customerRepo.Delete(customers);
            break;
        case "5":
            appointmentRepo.Create(appointments,customers);
            break;
        case "6":
            appointmentRepo.Read(appointments);
            break;
        case "7":
            appointmentRepo.Update(appointments);
            break;
        case "8":
            appointmentRepo.Delete(appointments);
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