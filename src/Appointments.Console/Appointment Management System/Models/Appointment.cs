using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Management_System.Models
{
    public class Appointment
    {
        private static int NextId = 1; //so as not to use guid
        private static int UsingResource = 0;
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public ServiceType ServiceType { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Notes { get; set; }

        public Appointment(Customer customer, ServiceType serviceType, DateTime date, string time, string notes)
        {
            Id = Interlocked.Increment(ref NextId);
            Customer = customer;
            ServiceType = serviceType;
            Date = date;
            Time = time;
            Notes = notes;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Customer: {Customer.Name}, Service: {ServiceType}, Date: {Date.ToShortDateString()}, Time: {Time}, Notes: {Notes}";
        }
    }
}
