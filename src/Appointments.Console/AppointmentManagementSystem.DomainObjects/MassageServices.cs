
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace AppointmentManagementSystem.DomainObjects
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;

    public class MassageServices
    {
        public required AppointmentManagementSystem.DomainObjects.Enums.MassageServices MassageServiceId { get; set; }
        public required string Name { get; set; }

        protected static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MassageServices>().HasKey(m => m.MassageServiceId);
            modelBuilder.Entity<MassageServices>().HasData(
                Enum.GetValues(typeof(AllEnums.MassageServices))
                    .Cast<AllEnums.MassageServices>()
                    .Select(e => new MassageServices()
                    {
                        MassageServiceId = e,
                        Name = e.ToString(),
                    })
            );

        }
    }
}
