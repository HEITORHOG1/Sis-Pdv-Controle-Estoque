using System.Configuration;

namespace Sis_Pdv_Controle_Estoque_Form.Utils
{
    public static class BaseAppConfig
    {
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";

                return result;
            }
            catch (ConfigurationErrorsException ex)
            {
                return ex.Message;
            }
        }
    }
}
