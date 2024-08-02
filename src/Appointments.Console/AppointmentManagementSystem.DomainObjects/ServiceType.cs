
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AppointmentManagementSystem.DomainObjects
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public class ServiceType
    {
        
        public required AppointmentManagementSystem.DomainObjects.Enums.ServiceType ServiceTypeId { get; set; }
        public required string Name { get; set; }
        protected static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceType>().HasData(
                  Enum.GetValues(typeof(AllEnums.ServiceType))
                      .Cast<AllEnums.ServiceType>()
                      .Select(e => new ServiceType()
                      {
                          ServiceTypeId = e,
                          Name = e.ToString(),
                      })
              );
        }

    }
}
