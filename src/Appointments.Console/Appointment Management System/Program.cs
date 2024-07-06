// See https://aka.ms/new-console-template for more information
using AppointmentManagementSystem;
using AppointmentManagementSystem.Interfaces;
using AppointmentManagementSystem.Models;
using AppointmentManagementSystem.Repositories;
using AppointmentManagementSystem.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
              .AddSingleton<IManagementRepository<Customer>, CustomerRepository>()
              //.AddSingleton<IManagementRepository<Appointment>, AppointmentRepository>()
              .AddSingleton<IDataEntryService, CustomerDataEntryService>()
              .BuildServiceProvider();

var dataEntryService = serviceProvider.GetService<IDataEntryService>();
//var customerRepo = serviceProvider.GetService<IManagementRepository<Customer>>();
//var appointmentRepo = serviceProvider.GetService<IManagementRepository<Appointment>>();

//if (customerRepo == null) throw new Exception("Customer repository is not initialized.");
//if (appointmentRepo == null) throw new Exception("Appointment repository is not initialized.");
if (dataEntryService== null) throw new Exception("Data Entry Service is not initialized.");

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

    string choice = Console.ReadLine()??"";
    Console.WriteLine();
    switch (choice)
    {
        case "1":
            dataEntryService.Create();
            break;
        case "2":
            dataEntryService.Read();
            break;
        case "3":
            dataEntryService.Update();
            break;
        case "4":
            dataEntryService.Delete();
            break;
        //case "5":
        //    appointmentRepo.Create(appointments,customers);
        //    break;
        //case "6":
        //    appointmentRepo.Read(appointments);
        //    break;
        //case "7":
        //    appointmentRepo.Update(appointments);
        //    break;
        //case "8":
        //    appointmentRepo.Delete(appointments);
        //    break;
        case "9":
            Console.WriteLine("Exiting... Press any key to close.");
            Console.ReadKey();
            return;
        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }
}