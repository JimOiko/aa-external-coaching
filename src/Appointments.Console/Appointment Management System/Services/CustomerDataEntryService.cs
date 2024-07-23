using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Infastructure.Interfaces;
using AppointmentManagementSystem.Interfaces;

namespace AppointmentManagementSystem.Services
{
    public class CustomerDataEntryService(ICustomerRepository customerRepo): ICustomerDataEntryService
    {
        private readonly ICustomerRepository _customerRepo = customerRepo;

        public void Create()
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

            var customer = new Customer(name, email, phoneNumber, DateTime.Now);
            _customerRepo.Add(customer);
        }

        public void Read()
        {
            var customers = _customerRepo.Get();
            Console.WriteLine("Customer List");
            Console.WriteLine("-------------");
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
            }
            else
            {
                Console.WriteLine("{0,-20} {1,-30} {2,-15}", "Name", "Email", "Phone Number");
                Console.WriteLine(new string('-', 65));
                foreach (var customer in customers)
                {
                    Console.WriteLine("{0,-20} {1,-30} {2,-15}", customer.Name, customer.Email, customer.PhoneNumber);
                }
            }
        }

        public void Update()
        {
            Console.WriteLine("Update Customer");
            Console.WriteLine("---------------");
            Console.Write("Enter the email of the customer to update: ");
            string email = Console.ReadLine() ?? "";

            var existingCustomer = _customerRepo.GetById(email);
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
                _customerRepo.Update(existingCustomer);
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        public void Delete()
        {
            Console.WriteLine("Delete Customer");
            Console.WriteLine("---------------");

            Console.Write("Enter the email of the customer to delete: ");
            string email = Console.ReadLine() ?? "";

            var customer = _customerRepo.GetById(email);
            if (customer != null)
            {
                _customerRepo.Delete(customer);
                Console.WriteLine("Customer deleted successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
    }
}
