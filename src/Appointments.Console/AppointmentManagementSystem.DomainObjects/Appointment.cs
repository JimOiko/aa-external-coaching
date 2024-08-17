using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using System.Text.Json.Serialization;

namespace AppointmentManagementSystem.DomainObjects
{
    //Define alias
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public class Appointment : ICloneable
    {

        // Parameterless constructor
        internal Appointment() { }

        public Appointment(Guid customerId, AllEnums.ServiceType serviceType, DateTimeOffset date, string? time, string? notes)
        {
            CustomerId = customerId;
            ServiceType = serviceType;
            Date = date;
            Time = time;
            Notes = notes;
        }

        [Key]
        [JsonPropertyName("appointmentId")]
        public Guid AppointmentId { get; set; } // Primary key

        [ForeignKey("Customer")]
        [JsonPropertyName("customerId")]
        public Guid CustomerId { get; set; }

        [JsonPropertyName("serviceType")]
        public AllEnums.ServiceType ServiceType { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }
        [JsonPropertyName("notes")]
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
