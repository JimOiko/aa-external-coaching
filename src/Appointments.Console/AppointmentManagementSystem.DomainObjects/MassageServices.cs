
using Microsoft.EntityFrameworkCore;

namespace AppointmentManagementSystem.DomainObjects
{
    using Constants = AppointmentManagementSystem.DomainObjects.Enums;

    public class MassageServices
    {
        public required AppointmentManagementSystem.DomainObjects.Enums.MassageServices MassageServiceId { get; set; }
        public required string Name { get; set; }

        protected static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MassageServices>().HasKey(m => m.MassageServiceId);
            modelBuilder.Entity<MassageServices>().HasData(
                Enum.GetValues(typeof(Constants.MassageServices))
                    .Cast<Constants.MassageServices>()
                    .Select(e => new MassageServices()
                    {
                        MassageServiceId = e,
                        Name = e.ToString(),
                    })
            );

        }
    }
}
