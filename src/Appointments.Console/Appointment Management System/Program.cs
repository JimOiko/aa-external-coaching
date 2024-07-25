// See https://aka.ms/new-console-template for more information
using AppManagementSystem.DbObjects;
using AppointmentManagementSystem;
using AppointmentManagementSystem.Infastructure;
using AppointmentManagementSystem.Infastructure.Interfaces;
using AppointmentManagementSystem.Interfaces;
using AppointmentManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
            .AddTransient<AppointmentManagementContext>()
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
    Console.WriteLine("11. Get Number of Appointments By Type");
    Console.WriteLine("12. Get Most Common Masseuse Preference Sex");
    Console.WriteLine("13. Get Most Common Preferenece for Training Duration On Personal");
    Console.WriteLine("14. Get Date with max Appointments for both Types");
    Console.WriteLine("15. Get Most Common Massage Service Preference");
    Console.WriteLine("16. Generate Appointments Day of Week Report");
    Console.WriteLine("17. Get New Customers On Specific Date");
    Console.WriteLine("18. Exit");
    Console.Write("Select an option (1-18): ");

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
            if (DateTimeOffset.TryParse(Console.ReadLine(), out DateTimeOffset date))
            {
                appointmentReportService.GetAppointmentsCountByDate(date);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
            break;
        case "11":
            appointmentReportService.GetNumberOfAppointmentsByType();
            break;
        case "12":
            appointmentReportService.GetCommonPreferenceForMasseuseSex();
            break;
        case "13":
            appointmentReportService.GetCommonPreferenceForTrainingDuration();
            break;
        case "14":
            appointmentReportService.GetMaxAppointmentsDateByServiceType();
            break;
        case "15":
            appointmentReportService.GetMassageTypePreference();
            break;
        case "16":
            appointmentReportService.GetAppointmentsDayOfWeekReport();
            break;
        case "17":
            Console.Write("Enter the date (yyyy-MM-dd): ");
            if (DateTimeOffset.TryParse(Console.ReadLine(), out DateTimeOffset dateRegistered))
            {
                customerReportService.GetNewCustomersByDate(dateRegistered);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
            break;
        case "18":
            Console.WriteLine("Exiting... Press any key to close.");
            Console.ReadKey();
            return;
        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }
}