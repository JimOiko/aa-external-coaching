using System.Text.RegularExpressions;

namespace AppointmentManagementSystem
{
    public class Utilities
    {
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length >= 10 && phoneNumber.Length <= 15 && long.TryParse(phoneNumber, out _);
        }
    }
}
