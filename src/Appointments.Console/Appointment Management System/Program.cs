// See https://aka.ms/new-console-template for more information
using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.DbObjects;
using Appointments.DAL;
using AppointmentManagementSystem.Abstractions;
using Customers.Client;
using Customers.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppointmentManagementSystem.DomainObjects.Interfaces;
using Appointments.Client;
using Appointments.BLL;


var configuration = new ConfigurationBuilder()
   .SetBasePath(Directory.GetCurrentDirectory())
   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
   .Build();

var serviceProvider = new ServiceCollection()
      .AddSingleton<IConfiguration>(configuration)
      .AddDbContext<AppointmentManagementContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("AppointmentManagementDatabase")), ServiceLifetime.Transient)
      .AddScoped<IDbContextFactory, DbContextFactory>()
              .AddSingleton<ICustomerRepository, CustomerRepository>()
              .AddSingleton<IAppointmentRepository, AppointmentRepository>()
              .AddSingleton<ICustomerDataEntryService, CustomerDataEntryService>()
              .AddSingleton<IAppointmentDataEntryService, AppointmentDataEntryService>()
              .AddSingleton<ICustomerReportService, CustomerReportService>()
              .AddSingleton<IAppointmentReportService, AppointmentReportService>()
              .AddSingleton<IDiscountService, DiscountService>()
              .AddSingleton<INameDayApiClient, NamedayApiClient>()
              .AddSingleton<IUserInputService, UserInputService>()
              .Configure<ApiSettings>(configuration.GetSection("ApiSettings"))
              .AddHttpClient()
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
        //can i not use the await in order to not majke the UI wait
        case "1":
            await customerDataEntryService.CreateAsync();
            break;
        case "2":
            await customerDataEntryService.ReadAsync();
            break;
        case "3":
            await customerDataEntryService.UpdateAsync();
            break;
        case "4":
            await customerDataEntryService.DeleteAsync();
            break;
        case "5":
            await appointmentDataEntryService.CreateAsync();
            break;
        case "6":
            await appointmentDataEntryService.ReadAsync();
            break;
        case "7":
            await appointmentDataEntryService.UpdateAsync();
            break;
        case "8":
            await appointmentDataEntryService.DeleteAsync();
            break;
        case "9":
            await customerReportService.GetRegisteredCustomerAsync();
            break;
        case "10":
            Console.Write("Enter the date (yyyy-MM-dd): ");
            if (DateTimeOffset.TryParse(Console.ReadLine(), out DateTimeOffset date))
            {
                var res = await appointmentReportService.GetAppointmentsCountByDateAsync(date);
                Console.WriteLine($"Total Number of Appointments on {date.DateTime.ToShortDateString()}: {res}");
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
            break;
        case "11":
            var (massageCount, ptCount) = await appointmentReportService.GetNumberOfAppointmentsByTypeAsync();
            Console.WriteLine($"Massage Count: {massageCount}, Personal Training Count: {ptCount}");
            break;
        case "12":
            var mPreference = await appointmentReportService.GetCommonPreferenceForMasseuseSexAsync();
            Console.WriteLine($"Most Common Preference for Masseuse Sex: {mPreference}");
            break;
        case "13":
            var tdPreference = await appointmentReportService.GetCommonPreferenceForTrainingDurationAsync();
            if (tdPreference != null)
                Console.WriteLine($"Most Common Preference for Personal Training Duration: {tdPreference}");
            else
                Console.WriteLine("Not enough Data to extract this Stat");
            break;
        case "14":
            var max = await appointmentReportService.GetMaxAppointmentsDateByServiceTypeAsync();
            foreach (var maxAppointment in max)
            {
                Console.WriteLine($"Service Type: {maxAppointment?.ServiceType}");
                if (maxAppointment?.Count != 0)
                {
                    Console.WriteLine($"Date with Max Appointments: {maxAppointment?.Date?.DateTime.ToShortDateString()}, Count: {maxAppointment?.Count}");
                }
                else
                {
                    Console.WriteLine("No appointments found for this service type.");
                }
            }
            break;
        case "15":
            var massageTypePref = await appointmentReportService.GetMassageTypePreferenceAsync();
            Console.WriteLine($"Most Common Preference for Massage Service: {massageTypePref}");
            break;

        case "16":
            var (maxDay, maxCount, minDay, minCount) = await appointmentReportService.GetAppointmentsDayOfWeekReportAsync();
            Console.WriteLine($"Day with Maximum Appointments: {maxDay}, Count: {maxCount}");
            Console.WriteLine($"Day with Minimum Appointments: {minDay}, Count: {minCount}");
            break;
        case "17":
            Console.Write("Enter the date (yyyy-MM-dd): ");
            if (DateTimeOffset.TryParse(Console.ReadLine(), out DateTimeOffset dateRegistered))
            {
                await customerReportService.GetNewCustomersByDateAsync(dateRegistered);
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