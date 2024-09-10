using Microsoft.EntityFrameworkCore;

namespace AppointmentManagementSystem.DomainObjects
{
    using Constants = AppointmentManagementSystem.DomainObjects.Enums;
    public class ServiceType
    {
        
        public required AppointmentManagementSystem.DomainObjects.Enums.ServiceType ServiceTypeId { get; set; }
        public required string Name { get; set; }
        protected static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceType>().HasData(
                  Enum.GetValues(typeof(Constants.ServiceType))
                      .Cast<Constants.ServiceType>()
                      .Select(e => new ServiceType()
                      {
                          ServiceTypeId = e,
                          Name = e.ToString(),
                      })
              );
        }

    }
}
