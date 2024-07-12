using AppointmentManagementSystem.Interfaces;
using AppointmentManagementSystem.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Services
{
    public class DataEntryServiceFactory : IDataEntryServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DataEntryServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDataEntryService Create(DataEntryServiceType serviceType)
        {
            return serviceType switch
            {
                DataEntryServiceType.Customer => _serviceProvider.GetRequiredService<CustomerDataEntryService>(),
                DataEntryServiceType.Appointment => _serviceProvider.GetRequiredService<AppointmentDataEntryService>(),
                _ => throw new ArgumentException("Invalid service type", nameof(serviceType))
            };
        }
    }
}
