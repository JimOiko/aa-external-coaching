﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppointmentManagementSystem.Models
{
    public class Customer(string name, string email, string phoneNumber, DateTime registrationDate) :ICloneable
    {
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
