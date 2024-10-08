﻿using AppointmentManagementSystem.DomainObjects;
using Customers.BLL.Interfaces;
using Customers.DAL.Interfaces;
using AppointmentManagementSystem;
using System.Net.Http.Json;
using Azure;
using Microsoft.Extensions.Options;
namespace Customers.BLL
{
    public class CustomerDataEntryService(HttpClient httpClient, IOptions<ApiSettings> apiSettings) : ICustomerDataEntryService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _apiSettings = apiSettings.Value;

        public async Task CreateAsync()
        {
            Console.WriteLine("Create Customer");
            Console.WriteLine("---------------");

            Console.Write("Enter Name: ");
            string name = Console.ReadLine() ?? "";

            string email;
            while (true)
            {
                Console.Write("Enter Email: ");
                email = Console.ReadLine() ?? "";
                if (Utilities.IsValidEmail(email))
                {
                    break;
                }
                Console.WriteLine("Invalid email format. Please try again.");
            }

            string phoneNumber;
            while (true)
            {
                Console.Write("Enter Phone Number: ");
                phoneNumber = Console.ReadLine() ?? "";
                if (Utilities.IsValidPhoneNumber(phoneNumber))
                {
                    break;
                }
                Console.WriteLine("Invalid phone number. Please try again.");
            }

            var customer = new Customer(name, email, phoneNumber, DateTimeOffset.Now);
            var response = await _httpClient.PostAsJsonAsync($"{_apiSettings.CustomerApiUrl}/create", customer);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Customer Created Successfully");
        }

        public async Task ReadAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiSettings.CustomerApiUrl}/get");
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

            var response = await _httpClient.GetAsync($"{_apiSettings.CustomerApiUrl}/getByEmail/{email}");
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
                await _httpClient.PutAsJsonAsync($"{_apiSettings.CustomerApiUrl}/update/{existingCustomer.Id}", existingCustomer);
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
            var deleteResponse = await _httpClient.DeleteAsync($"{_apiSettings.CustomerApiUrl}/delete/{email}");
            deleteResponse.EnsureSuccessStatusCode();

            Console.WriteLine("Customer deleted successfully.");
        }
    }
}
