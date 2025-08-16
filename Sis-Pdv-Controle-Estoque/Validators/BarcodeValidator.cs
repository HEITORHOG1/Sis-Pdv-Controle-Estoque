using System.Text.RegularExpressions;

namespace Validators
{
    public static class BarcodeValidator
    {
        /// <summary>
        /// Validates EAN-13 barcode format (13 digits)
        /// </summary>
        public static bool IsValidEan13(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode) || barcode.Length != 13)
                return false;

            if (!barcode.All(char.IsDigit))
                return false;

            // Calculate check digit
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(barcode[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == int.Parse(barcode[12].ToString());
        }

        /// <summary>
        /// Validates EAN-8 barcode format (8 digits)
        /// </summary>
        public static bool IsValidEan8(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode) || barcode.Length != 8)
                return false;

            if (!barcode.All(char.IsDigit))
                return false;

            // Calculate check digit
            int sum = 0;
            for (int i = 0; i < 7; i++)
            {
                int digit = int.Parse(barcode[i].ToString());
                sum += (i % 2 == 0) ? digit * 3 : digit;
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == int.Parse(barcode[7].ToString());
        }

        /// <summary>
        /// Validates UPC-A barcode format (12 digits)
        /// </summary>
        public static bool IsValidUpcA(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode) || barcode.Length != 12)
                return false;

            if (!barcode.All(char.IsDigit))
                return false;

            // Calculate check digit
            int sum = 0;
            for (int i = 0; i < 11; i++)
            {
                int digit = int.Parse(barcode[i].ToString());
                sum += (i % 2 == 0) ? digit * 3 : digit;
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == int.Parse(barcode[11].ToString());
        }

        /// <summary>
        /// Validates common barcode formats (EAN-13, EAN-8, UPC-A)
        /// </summary>
        public static bool IsValidBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return false;

            return barcode.Length switch
            {
                8 => IsValidEan8(barcode),
                12 => IsValidUpcA(barcode),
                13 => IsValidEan13(barcode),
                _ => false
            };
        }

        /// <summary>
        /// Validates internal product code format (alphanumeric, 6-20 characters)
        /// </summary>
        public static bool IsValidInternalCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            if (code.Length < 6 || code.Length > 20)
                return false;

            return Regex.IsMatch(code, @"^[A-Za-z0-9]+$");
        }
    }
}