using System.Text.RegularExpressions;

namespace Validators
{
    public static class BusinessValidator
    {
        /// <summary>
        /// Validates email format
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
                return emailRegex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates Brazilian phone number format
        /// </summary>
        public static bool IsValidBrazilianPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Remove formatting
            var cleanPhone = Regex.Replace(phone, @"[^\d]", "");

            // Brazilian phone: 10 digits (landline) or 11 digits (mobile)
            return cleanPhone.Length == 10 || cleanPhone.Length == 11;
        }

        /// <summary>
        /// Validates Brazilian CEP (postal code) format
        /// </summary>
        public static bool IsValidCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return false;

            // Remove formatting
            var cleanCep = cep.Replace("-", "").Replace(".", "");

            return cleanCep.Length == 8 && cleanCep.All(char.IsDigit);
        }

        /// <summary>
        /// Validates price/monetary value (positive decimal with up to 2 decimal places)
        /// </summary>
        public static bool IsValidPrice(decimal price)
        {
            return price >= 0 && decimal.Round(price, 2) == price;
        }

        /// <summary>
        /// Validates quantity (positive integer or decimal)
        /// </summary>
        public static bool IsValidQuantity(decimal quantity)
        {
            return quantity >= 0;
        }

        /// <summary>
        /// Validates percentage (0-9999.99)
        /// </summary>
        public static bool IsValidPercentage(decimal percentage)
        {
            // Muitos produtos podem ter margem acima de 100% (ex.: custo 5, venda 11 => 120%).
            // Ampliamos o limite superior para alinhar com o cliente (até 9999.99%).
            return percentage >= 0 && percentage <= 9999.99m;
        }

        /// <summary>
        /// Validates that a string contains only letters and spaces
        /// </summary>
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return Regex.IsMatch(name.Trim(), @"^[a-zA-ZÀ-ÿ\s]+$");
        }

        /// <summary>
        /// Validates that a date is not in the future (for birth dates, etc.)
        /// </summary>
        public static bool IsValidPastDate(DateTime date)
        {
            return date <= DateTime.Now;
        }

        /// <summary>
        /// Validates that a date is within a reasonable business range
        /// </summary>
        public static bool IsValidBusinessDate(DateTime date)
        {
            var minDate = DateTime.Now.AddYears(-100);
            var maxDate = DateTime.Now.AddYears(10);
            return date >= minDate && date <= maxDate;
        }
    }
}