﻿
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagementSystem.DomainObjects
{
    public class Customer(string name, string email, string phoneNumber, DateTimeOffset registrationDate) :ICloneable
    {
        [Key]
        public Guid Id { get; set; } // Primary key

        public string Name { get; set; } = name;
        public string Email { get; set; } = email;
        public string PhoneNumber { get; set; } = phoneNumber;
        public DateTimeOffset RegistrationDate { get; set; } = registrationDate;
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"Name: {Name}, Email: {Email}, Phone Number: {PhoneNumber}, Registration Date: {RegistrationDate}";
        }
    }
}
