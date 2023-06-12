namespace Sis_Pdv_Controle_Estoque_Form.Utils
{
    public abstract class RemoverPontosCpf
    {
        public static string RemoverCaracteresCpf(string cpf)
        {
            return cpf.Replace(".", "").Replace("-", "");
        }
    }
}
