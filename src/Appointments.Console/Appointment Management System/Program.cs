// See https://aka.ms/new-console-template for more information
using AppointmentManagementSystem;
using AppointmentManagementSystem.Interfaces;
using AppointmentManagementSystem.Models;
using AppointmentManagementSystem.Repositories;
using AppointmentManagementSystem.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
              .AddSingleton<IManagementRepository<Customer>, CustomerRepository>()
              .AddSingleton<IManagementRepository<Appointment>, AppointmentRepository>()
              .AddSingleton<CustomerDataEntryService>()
              .AddSingleton<AppointmentDataEntryService>()
              .AddSingleton<IDataEntryService, CustomerDataEntryService>()
              .AddSingleton<IDataEntryService, AppointmentDataEntryService>()
              .AddSingleton<IDataEntryServiceFactory, DataEntryServiceFactory>()
              .BuildServiceProvider();

var factory = serviceProvider.GetRequiredService<IDataEntryServiceFactory>();
var customerDataEntryService = factory.Create(DataEntryServiceType.Customer);
var appointmentDataEntryService = factory.Create(DataEntryServiceType.Appointment);

if (customerDataEntryService == null) throw new NullReferenceException("Customer Data Entry Service is not initialized.");
if (appointmentDataEntryService == null) throw new NullReferenceException("Appointment Data Entry Service is not initialized.");

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
            customerDataEntryService.Create();
            break;
        case "2":
            customerDataEntryService.Read();
            break;
        case "3":
            customerDataEntryService.Update();
            break;
        case "4":
            customerDataEntryService.Delete();
            break;
        case "5":
            appointmentDataEntryService.Create();
            break;
        case "6":
            appointmentDataEntryService.Read();
            break;
        case "7":
            appointmentDataEntryService.Update();
            break;
        case "8":
            appointmentDataEntryService.Delete();
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