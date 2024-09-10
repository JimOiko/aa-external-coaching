using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentManagementSystem.DomainObjects
{
    //Define alias
    using Constants = AppointmentManagementSystem.DomainObjects.Enums;
    public class Appointment : ICloneable
    {

        // Parameterless constructor
        internal Appointment() { }

        public Appointment(Guid customerId, Constants.ServiceType serviceType, DateTimeOffset date, string? time, string? notes)
        {
            CustomerId = customerId;
            ServiceType = serviceType;
            Date = date;
            Time = time;
            Notes = notes;
        }

        [Key]
        public Guid AppointmentId { get; set; } // Primary key

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        public Constants.ServiceType ServiceType { get; set; }
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

        protected static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().UseTptMappingStrategy();

            modelBuilder
                .Entity<Appointment>()
                .Property(e => e.ServiceType)
                .HasConversion<int>();

            modelBuilder.Entity<Appointment>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .IsRequired();
        }

    }
}
