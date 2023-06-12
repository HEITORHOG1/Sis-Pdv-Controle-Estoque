using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_Form.Dto.TratamentoErro
{
    public static class ErrorHandling
    {
        public static class ErrorLogger
        {
            public static List<string> ErrorMessages { get; private set; }

            static ErrorLogger()
            {
                ErrorMessages = new List<string>();
            }

            public static void LogError(string errorMessage)
            {
                ErrorMessages.Add(errorMessage);
                // Aqui você pode adicionar lógica adicional, como gravar os erros em um arquivo de log
            }

            public static void ClearErrors()
            {
                ErrorMessages.Clear();
            }
        }
    }

}
