using AppointmentManagementSystem.Interfaces;
using AppointmentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Repositories
{
    public class CustomerRepository: IManagementRepository<Customer>
    {
         public void Create(List<Customer> customers, List<Customer>? item = null)
        {
            Console.WriteLine("Create Customer");
            Console.WriteLine("---------------");

            Console.Write("Enter Name:");
            string name = Console.ReadLine()??"";
            string email;
            while (true)
            {
                Console.Write("Enter Email: ");
                email = Console.ReadLine()??"";
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
                phoneNumber = Console.ReadLine()??"";
                if (Utilities.IsValidPhoneNumber(phoneNumber))
                {
                    break;
                }
                Console.WriteLine("Invalid phone number. Please try again.");
            }

            customers.Add(new Customer(name, email, phoneNumber));
            Console.WriteLine("Customer created successfully.");
        }

        public void Read(List<Customer> customers)
        {
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

        public void Update(List<Customer> customers)
        {
            Console.WriteLine("Update Customer");
            Console.WriteLine("---------------");
            Console.Write("Enter the email of the customer to update: ");
            string email = Console.ReadLine()??"";

            Customer? customer = customers.Find(c => c.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
            if (customer != null)
            {
                Console.Write("Enter new Name: ");
                customer.Name = Console.ReadLine()??"";
                Console.Write("Enter new Email: ");
                customer.Email = Console.ReadLine()??"";
                Console.Write("Enter new Phone Number: ");
                customer.PhoneNumber = Console.ReadLine()??"";

                Console.WriteLine("Customer updated successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        public void Delete(List<Customer> customers)
        {
            Console.WriteLine("Delete Customer");
            Console.WriteLine("---------------");

            Console.Write("Enter the email of the customer to delete: ");
            string email = Console.ReadLine()??"";

            Customer? customer = customers.Find(c => c.Email.ToLower() == email.ToLower());
            if (customer != null)
            {
                customers.Remove(customer);
                Console.WriteLine("Customer deleted successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
    }
}
