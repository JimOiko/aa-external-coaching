using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentManagementSystem.DomainObjects
{
    //Define alias
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public class Appointment : ICloneable
    {

        // Parameterless constructor
        internal Appointment() { }

        public Appointment(int customerId, AllEnums.ServiceType serviceType, DateTimeOffset date, string? time, string? notes)
        {
            CustomerId = customerId;
            ServiceType = serviceType;
            Date = date;
            Time = time;
            Notes = notes;
        }

        [Key]
        public int AppointmentId { get; set; } // Primary key

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public AllEnums.ServiceType ServiceType { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Time { get; set; }
        public string? Notes { get; set; }

        public override string ToString()
        {
            return $"ID: {AppointmentId}, Service: {ServiceType}, Date: {Date.DateTime.ToShortDateString()}, Time: {Time}, Notes: {Notes}";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

    }
}
