using Sis_Pdv_Controle_Estoque_Form.Paginas.Login;

namespace Sis_Pdv_Controle_Estoque_Form
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Application.Run(new frmLogin());
        }
    }
}