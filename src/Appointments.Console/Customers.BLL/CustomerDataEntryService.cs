using AppointmentManagementSystem.DomainObjects;
using Customers.BLL.Interfaces;
using Customers.DAL.Interfaces;
using AppointmentManagementSystem;
using System.Net.Http.Json;
using Azure;
using AppointmentManagementSystem.DomainObjects.Interfaces;
namespace Customers.BLL
{
    public class CustomerDataEntryService(HttpClient httpClient, IUserInputService userInputService) : ICustomerDataEntryService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IUserInputService _userInputService = userInputService;

        public async Task<object> CreateAsync()
        {
            Console.WriteLine("Create Customer");
            Console.WriteLine("---------------");

            Console.Write("Enter Name: ");
            string name = _userInputService.ReadLine();

            string email;
            while (true)
            {
                Console.Write("Enter Email: ");
                email = _userInputService.ReadLine();
                if (Utilities.IsValidEmail(email))
                {
                    break;
                }
                Console.WriteLine("Invalid email format. Please try again.");
            }

            string phoneNumber;
            Console.Write("Enter Phone Number: ");
            phoneNumber = _userInputService.ReadLine();
            if (!Utilities.IsValidPhoneNumber(phoneNumber))
            {
                Console.WriteLine("Invalid phone number. Please try again.");
                return null;
            }

            var customer = new Customer(name, email, phoneNumber, DateTimeOffset.Now);
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7211/api/customers/create", customer);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Customer Created Successfully");
            return customer;
        }

        public async Task ReadAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7211/api/customers/get");
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Customer List");
            Console.WriteLine("-------------");
            var customers = await response.Content.ReadFromJsonAsync<List<Customer>>();
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
            }
            else
            {
                Console.WriteLine("{0,-20} {1,-30} {2,-15} {3,-20}", "Name", "Email", "Phone Number", "Registration Date");
                Console.WriteLine(new string('-', 95));
                foreach (var customer in customers)
                {
                    Console.WriteLine("{0,-20} {1,-30} {2,-15} {3,-20}", customer.Name, customer.Email, customer.PhoneNumber, customer.RegistrationDate);
                }
            }
        }

        public async Task UpdateAsync()
        {
            Console.WriteLine("Update Customer");
            Console.WriteLine("---------------");
            Console.Write("Enter the email of the customer to update: ");
            string email = Console.ReadLine() ?? "";

            var response = await _httpClient.GetAsync($"https://localhost:7211/api/customers/getByEmail/{email}");
            response.EnsureSuccessStatusCode();

            var existingCustomer = await response.Content.ReadFromJsonAsync<Customer>();
            if (existingCustomer != null)
            {
                Console.Write("Enter new Name: ");
                existingCustomer.Name = Console.ReadLine() ?? "";
                string newEmail;
                while (true)
                {
                    Console.Write("Enter new Email: ");
                    newEmail = Console.ReadLine() ?? "";
                    if (Utilities.IsValidEmail(email))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid email format. Please try again.");
                }
                existingCustomer.Email = newEmail;
                Console.Write("Enter new Phone Number: ");
                string phoneNumber;
                while (true)
                {
                    phoneNumber = Console.ReadLine() ?? "";
                    if (Utilities.IsValidPhoneNumber(phoneNumber))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid phone number. Please try again.");
                }
                existingCustomer.PhoneNumber = phoneNumber;
                await _httpClient.PutAsJsonAsync($"https://localhost:7211/api/customers/update/{existingCustomer.Id}", existingCustomer);
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        public async Task DeleteAsync()
        {
            Console.WriteLine("Delete Customer");
            Console.WriteLine("---------------");

            string email;
            while (true)
            {
                Console.Write("Enter new Email: ");
                email = Console.ReadLine() ?? "";
                if (Utilities.IsValidEmail(email))
                {
                    break;
                }
                Console.WriteLine("Invalid email format. Please try again.");
            }
            var deleteResponse = await _httpClient.DeleteAsync($"https://localhost:7211/api/customers/delete/{email}");
            deleteResponse.EnsureSuccessStatusCode();

            Console.WriteLine("Customer deleted successfully.");
        }
    }
}
