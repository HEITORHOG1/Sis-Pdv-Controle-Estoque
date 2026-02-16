namespace Sis_Pdv_Controle_Estoque_Form.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Formata uma lista de strings de erro para exibição
        /// </summary>
        public static string FormatErrorMessages(this List<string> errors)
        {
            if (errors == null || !errors.Any())
                return "Erro desconhecido";

            return string.Join("\n• ", errors.Where(e => !string.IsNullOrWhiteSpace(e)));
        }

        /// <summary>
        /// Formata uma única mensagem de erro
        /// </summary>
        public static string FormatErrorMessage(this string error)
        {
            if (string.IsNullOrWhiteSpace(error))
                return "Erro desconhecido";

            return error.Trim();
        }

        /// <summary>
        /// Capitaliza a primeira letra de uma string
        /// </summary>
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        /// <summary>
        /// Remove caracteres especiais de uma string
        /// </summary>
        public static string RemoveSpecialCharacters(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return System.Text.RegularExpressions.Regex.Replace(input, @"[^a-zA-Z0-9\s]", "");
        }

        /// <summary>
        /// Formata uma string para exibição amigável
        /// </summary>
        public static string ToFriendlyString(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "N/A";

            return input.Trim().Capitalize();
        }

        /// <summary>
        /// Trunca uma string se ela for muito longa
        /// </summary>
        public static string Truncate(this string input, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length <= maxLength)
                return input;

            return input.Substring(0, maxLength - suffix.Length) + suffix;
        }

        /// <summary>
        /// Verifica se uma string é um email válido
        /// </summary>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica se uma string contém apenas números
        /// </summary>
        public static bool IsNumeric(this string input)
        {
            return !string.IsNullOrWhiteSpace(input) && input.All(char.IsDigit);
        }

        /// <summary>
        /// Remove acentos de uma string
        /// </summary>
        public static string RemoveAccents(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var normalizedString = input.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

        /// <summary>
        /// Converte string para formato de telefone brasileiro
        /// </summary>
        public static string FormatPhoneNumber(this string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return phone;

            var numbersOnly = new string(phone.Where(char.IsDigit).ToArray());

            return numbersOnly.Length switch
            {
                10 => $"({numbersOnly.Substring(0, 2)}) {numbersOnly.Substring(2, 4)}-{numbersOnly.Substring(6)}",
                11 => $"({numbersOnly.Substring(0, 2)}) {numbersOnly.Substring(2, 5)}-{numbersOnly.Substring(7)}",
                _ => phone
            };
        }

        /// <summary>
        /// Converte string para formato de CPF
        /// </summary>
        public static string FormatCPF(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return cpf;

            var numbersOnly = new string(cpf.Where(char.IsDigit).ToArray());

            if (numbersOnly.Length == 11)
            {
                return $"{numbersOnly.Substring(0, 3)}.{numbersOnly.Substring(3, 3)}.{numbersOnly.Substring(6, 3)}-{numbersOnly.Substring(9)}";
            }

            return cpf;
        }

        /// <summary>
        /// Converte string para formato de CNPJ
        /// </summary>
        public static string FormatCNPJ(this string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return cnpj;

            var numbersOnly = new string(cnpj.Where(char.IsDigit).ToArray());

            if (numbersOnly.Length == 14)
            {
                return $"{numbersOnly.Substring(0, 2)}.{numbersOnly.Substring(2, 3)}.{numbersOnly.Substring(5, 3)}/{numbersOnly.Substring(8, 4)}-{numbersOnly.Substring(12)}";
            }

            return cnpj;
        }

        /// <summary>
        /// Converte string para formato de CEP
        /// </summary>
        public static string FormatCEP(this string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return cep;

            var numbersOnly = new string(cep.Where(char.IsDigit).ToArray());

            if (numbersOnly.Length == 8)
            {
                return $"{numbersOnly.Substring(0, 5)}-{numbersOnly.Substring(5)}";
            }

            return cep;
        }

        /// <summary>
        /// Converte primeira letra de cada palavra para maiúscula
        /// </summary>
        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        /// <summary>
        /// Verifica se a string é nula, vazia ou contém apenas espaços
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// Retorna a string ou um valor padrão se for nula/vazia
        /// </summary>
        public static string OrDefault(this string input, string defaultValue = "N/A")
        {
            return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
        }
    }
}