using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Models
{
    public class Appointment(Customer customer, ServiceType serviceType, DateTime date, string time, string notes) : ICloneable
    {
        private static int NextId = 1; //so as not to use guid
        public int Id { get; set; } = Interlocked.Increment(ref NextId);
        public Customer Customer { get; set; } = customer;
        public ServiceType ServiceType { get; set; } = serviceType;
        public DateTime Date { get; set; } = date;
        public string Time { get; set; } = time;
        public string Notes { get; set; } = notes;

        public override string ToString()
        {
            return $"ID: {Id}, Customer: {Customer.Name}, Service: {ServiceType}, Date: {Date.ToShortDateString()}, Time: {Time}, Notes: {Notes}";
        }

        public object Clone()
        {
            return MemberwiseClone() ;
        }
    }
}
