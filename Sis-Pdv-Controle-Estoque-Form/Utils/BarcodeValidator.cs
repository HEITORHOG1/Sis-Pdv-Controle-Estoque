using System;

namespace Sis_Pdv_Controle_Estoque_Form.Utils
{
    public static class BarcodeValidator
    {
        // EAN-13 (13 dígitos)
        public static bool IsValidEan13(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode) || barcode.Length != 13) return false;
            if (!AllDigits(barcode)) return false;

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = barcode[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (barcode[12] - '0');
        }

        // EAN-8 (8 dígitos)
        public static bool IsValidEan8(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode) || barcode.Length != 8) return false;
            if (!AllDigits(barcode)) return false;

            int sum = 0;
            for (int i = 0; i < 7; i++)
            {
                int digit = barcode[i] - '0';
                sum += (i % 2 == 0) ? digit * 3 : digit;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (barcode[7] - '0');
        }

        // UPC-A (12 dígitos)
        public static bool IsValidUpcA(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode) || barcode.Length != 12) return false;
            if (!AllDigits(barcode)) return false;

            int sum = 0;
            for (int i = 0; i < 11; i++)
            {
                int digit = barcode[i] - '0';
                sum += (i % 2 == 0) ? digit * 3 : digit;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (barcode[11] - '0');
        }

        // Principal
        public static bool IsValidBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return false;
            return barcode.Length switch
            {
                8 => IsValidEan8(barcode),
                12 => IsValidUpcA(barcode),
                13 => IsValidEan13(barcode),
                _ => false
            };
        }

        private static bool AllDigits(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] < '0' || s[i] > '9') return false;
            }
            return true;
        }
    }
}
