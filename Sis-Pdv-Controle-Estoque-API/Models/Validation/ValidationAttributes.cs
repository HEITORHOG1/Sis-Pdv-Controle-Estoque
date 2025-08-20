using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sis_Pdv_Controle_Estoque_API.Models.Validation
{
    /// <summary>
    /// Validates barcode format (8-20 digits)
    /// </summary>
    public class BarcodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not string barcode)
                return false;

            if (string.IsNullOrWhiteSpace(barcode))
                return false;

            // Check if it's between 8-20 digits
            return Regex.IsMatch(barcode, @"^\d{8,20}$");
        }

        public override string FormatErrorMessage(string name)
        {
            return $"O campo {name} deve conter entre 8 e 20 dígitos numéricos.";
        }
    }

    /// <summary>
    /// Validates that a decimal value is positive
    /// </summary>
    public class PositiveDecimalAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is decimal decimalValue)
                return decimalValue > 0;
            
            if (value is double doubleValue)
                return doubleValue > 0;
            
            if (value is float floatValue)
                return floatValue > 0;

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"O campo {name} deve ser um valor positivo.";
        }
    }

    /// <summary>
    /// Validates that a quantity is not negative
    /// </summary>
    public class NonNegativeQuantityAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is int intValue)
                return intValue >= 0;
            
            if (value is decimal decimalValue)
                return decimalValue >= 0;

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"O campo {name} não pode ser negativo.";
        }
    }

    /// <summary>
    /// Validates that a GUID is not empty
    /// </summary>
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is Guid guidValue)
                return guidValue != Guid.Empty;

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"O campo {name} é obrigatório e deve ser um GUID válido.";
        }
    }

    /// <summary>
    /// Validates product name format and length
    /// </summary>
    public class ProductNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not string name)
                return false;

            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Must be between 2 and 100 characters
            if (name.Trim().Length < 2 || name.Trim().Length > 100)
                return false;

            // Cannot contain only special characters
            return Regex.IsMatch(name.Trim(), @"[a-zA-Z0-9À-ÿ]");
        }

        public override string FormatErrorMessage(string name)
        {
            return $"O campo {name} deve ter entre 2 e 100 caracteres e conter pelo menos uma letra ou número.";
        }
    }

    /// <summary>
    /// Validates stock movement reason
    /// </summary>
    public class MovementReasonAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not string reason)
                return false;

            if (string.IsNullOrWhiteSpace(reason))
                return false;

            // Must be between 5 and 500 characters
            return reason.Trim().Length >= 5 && reason.Trim().Length <= 500;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"O campo {name} deve ter entre 5 e 500 caracteres.";
        }
    }
}