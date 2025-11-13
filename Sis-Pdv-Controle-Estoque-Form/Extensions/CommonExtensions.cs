namespace Sis_Pdv_Controle_Estoque_Form.Extensions
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Formata mensagens de erro para exibição
        /// </summary>
        public static string FormatErrorMessages(this IEnumerable<string> errors)
        {
            if (errors == null || !errors.Any())
                return "Erro desconhecido";

            return string.Join("\n• ", errors.Select(e => e.Trim()));
        }

        /// <summary>
        /// Valida se uma string é um GUID válido
        /// </summary>
        public static bool IsValidGuid(this string input)
        {
            return !string.IsNullOrWhiteSpace(input) && Guid.TryParse(input, out _);
        }

        /// <summary>
        /// Remove caracteres especiais de texto mantendo apenas letras, números e espaços
        /// </summary>
        public static string RemoveSpecialCharacters(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return new string(input.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
        }

        /// <summary>
        /// Capitaliza a primeira letra de cada palavra
        /// </summary>
        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        /// <summary>
        /// Remove espaços múltiplos deixando apenas um espaço entre palavras
        /// </summary>
        public static string NormalizeWhitespace(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return System.Text.RegularExpressions.Regex.Replace(input.Trim(), @"\s+", " ");
        }
    }
}