using Model.Exceptions;

namespace Model
{
    public class Cliente : EntityBase
    {
        // Construtor para EF Core
        public Cliente()
        {
        }

        public Cliente(string cpfCnpj, string tipoCliente)
        {
            ValidarCpfCnpj(cpfCnpj);
            ValidarTipoCliente(tipoCliente);
            
            CpfCnpj = cpfCnpj;
            TipoCliente = tipoCliente;
        }
        
        public string CpfCnpj { get; set; }
        public string TipoCliente { get; set; }
        
        private static void ValidarCpfCnpj(string cpfCnpj)
        {
            if (string.IsNullOrWhiteSpace(cpfCnpj))
                throw new DomainException("CPF/CNPJ é obrigatório");
            
            // Remove caracteres não numéricos
            var apenasNumeros = new string(cpfCnpj.Where(char.IsDigit).ToArray());
            
            if (apenasNumeros.Length != 11 && apenasNumeros.Length != 14)
                throw new DomainException("CPF/CNPJ inválido. Deve conter 11 dígitos (CPF) ou 14 dígitos (CNPJ)");
        }
        
        private static void ValidarTipoCliente(string tipoCliente)
        {
            if (string.IsNullOrWhiteSpace(tipoCliente))
                throw new DomainException("Tipo de cliente é obrigatório");
        }
        
        public void AtualizarDados(string cpfCnpj, string tipoCliente)
        {
            ValidarCpfCnpj(cpfCnpj);
            ValidarTipoCliente(tipoCliente);
            
            CpfCnpj = cpfCnpj;
            TipoCliente = tipoCliente;
        }
    }
}
