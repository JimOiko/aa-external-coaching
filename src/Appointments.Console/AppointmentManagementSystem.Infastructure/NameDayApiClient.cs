using AppointmentManagementSystem.Infastructure.Interfaces;
using Microsoft.Extensions.Options;
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
        private readonly ApiSettings _apiSettings;

        public NamedayApiClient(HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }

        public async Task<string> GetNamedayAsync(DateTimeOffset date)
        {
            string day = date.Day.ToString();
            string month = date.Month.ToString();

            var url = $"{_apiSettings.NameDayApiBaseUrl}?day={day}&month={month}&country={_apiSettings.DefaultCountry}";
            var response = await _httpClient.GetAsync(url);
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
