// See https://aka.ms/new-console-template for more information
using AppointmentManagementSystem;
using AppointmentManagementSystem.Interfaces;
using AppointmentManagementSystem.Models;
using AppointmentManagementSystem.Repositories;
using AppointmentManagementSystem.Services;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
              .AddSingleton<ICustomerRepository, CustomerRepository>()
              .AddSingleton<IAppointmentRepository, AppointmentRepository>()
              .AddSingleton<ICustomerDataEntryService, CustomerDataEntryService>()
              .AddSingleton<IAppointmentDataEntryService, AppointmentDataEntryService>()
              .AddSingleton<ICustomerReportService, CustomerReportService>()
              .AddSingleton<IAppointmentReportService, AppointmentReportService>()
              .BuildServiceProvider();


var customerDataEntryService = serviceProvider.GetService<ICustomerDataEntryService>();
var appointmentDataEntryService = serviceProvider.GetService<IAppointmentDataEntryService>();
var customerReportService = serviceProvider.GetRequiredService<ICustomerReportService>();
var appointmentReportService = serviceProvider.GetRequiredService<IAppointmentReportService>();

if (customerDataEntryService == null) throw new NullReferenceException("Customer Data Entry Service is not initialized.");
if (appointmentDataEntryService == null) throw new NullReferenceException("Appointment Data Entry Service is not initialized.");

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
    Console.WriteLine("9. Generate Customer Report");
    Console.WriteLine("10. Generate Appointment Report by Date");
    Console.WriteLine("11. Exit");
    Console.Write("Select an option (1-11): ");

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
            customerReportService.GetRegisteredCustomer();
            break;
        case "10":
            Console.Write("Enter the date (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                appointmentReportService.GetAppointmentsCountByDate(date);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
            break;
        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }
}