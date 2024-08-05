using AppManagementSystem.DbObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.DbObjects
{
    public interface IDbContextFactory
    {
        AppointmentManagementContext CreateDbContext();
    }

    public class DbContextFactory : IDbContextFactory
    {
        private readonly DbContextOptions<AppointmentManagementContext> _options;

        public DbContextFactory(DbContextOptions<AppointmentManagementContext> options)
        {
            _options = options;
        }

        public AppointmentManagementContext CreateDbContext()
        {
            return new AppointmentManagementContext(_options,null);
        }
    }
}
