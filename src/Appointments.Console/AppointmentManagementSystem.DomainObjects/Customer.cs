
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagementSystem.DomainObjects
{
    public class Customer(string name, string email, string phoneNumber, DateTime registrationDate) :ICloneable
    {
        [Key]
        public int Id { get; set; } // Primary key

        public string Name { get; set; } = name;
        public string Email { get; set; } = email;
        public string PhoneNumber { get; set; } = phoneNumber;
        public DateTime RegistrationDate { get; set; } = registrationDate;
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"Name: {Name}, Email: {Email}, Phone Number: {PhoneNumber}";
        }
    }
}
