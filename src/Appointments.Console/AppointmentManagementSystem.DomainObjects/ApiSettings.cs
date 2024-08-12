using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.DomainObjects
{
    public class ApiSettings
    {
        public required string NameDayApiBaseUrl { get; set; }
        public required string DefaultCountry { get; set; }
    }
}
