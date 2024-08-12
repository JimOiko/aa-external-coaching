using AppointmentManagementSystem.DomainObjects;
using Customers.BLL.Interfaces;
using Customers.DAL.Interfaces;
using AppointmentManagementSystem;
namespace Customers.BLL
{
    public class CustomerDataEntryService(ICustomerRepository customerRepo): ICustomerDataEntryService
    {
        private readonly ICustomerRepository _customerRepo = customerRepo;

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
            await _customerRepo.AddAsync(customer);
            Console.WriteLine("Customer Created Successfully");
        }

        public async Task ReadAsync()
        {
            var customers = await _customerRepo.GetAsync();
            Console.WriteLine("Customer List");
            Console.WriteLine("-------------");
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

            var existingCustomer = await _customerRepo.GetByEmailAsync(email);
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
                await _customerRepo.UpdateAsync(existingCustomer);
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

            Console.Write("Enter the email of the customer to delete: ");
            string email = Console.ReadLine() ?? "";

            var customer = await _customerRepo.GetByEmailAsync(email);
            if (customer != null)
            {
                await _customerRepo.DeleteAsync(customer);
                Console.WriteLine("Customer deleted successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
    }
}
