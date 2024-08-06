using AppointmentManagementSystem.Infastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Infastructure
{
    public class NamedayApiClient:INameDayApiClient
    {
        private readonly HttpClient _httpClient;

        public NamedayApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetNamedayAsync(DateTimeOffset date)
        {
            string day = date.Day.ToString();
            string month = date.Month.ToString();

            var response = await _httpClient.GetAsync($"https://nameday.abalin.net/api/V1/getdate?day={day}&month={month}&country=gr"); //country can become dynamic
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse the JSON response using System.Text.Json
            using var jsonDocument = JsonDocument.Parse(jsonResponse);
            if (jsonDocument.RootElement.TryGetProperty("nameday", out var namedaysElement))
            {
                return namedaysElement.ToString();
            }

            return string.Empty;
        }
    }
}
