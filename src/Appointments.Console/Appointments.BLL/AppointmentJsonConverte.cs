using AppointmentManagementSystem.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Appointments.BLL
{
    public class AppointmentJsonConverter : JsonConverter<Appointment>
    {
        public override Appointment Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDoc = JsonDocument.ParseValue(ref reader);
            var rootElement = jsonDoc.RootElement;

            var serviceType = rootElement.GetProperty("serviceType").GetInt32();
            var rawText = rootElement.GetRawText();

            return serviceType switch
            {
                0 => JsonSerializer.Deserialize<MassageAppointment>(rawText, options),
                1 => JsonSerializer.Deserialize<PersonalTrainingAppointment>(rawText, options),
                _ => JsonSerializer.Deserialize<Appointment>(rawText, options),
            };
        }

        public override void Write(Utf8JsonWriter writer, Appointment value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
        }
    }
}
