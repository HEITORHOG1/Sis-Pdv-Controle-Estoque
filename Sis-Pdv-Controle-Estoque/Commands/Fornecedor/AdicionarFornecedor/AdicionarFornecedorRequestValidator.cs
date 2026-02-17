using FluentValidation;
using Validators;

namespace Commands.Fornecedor.AdicionarFornecedor
{
    public class AdicionarFornecedorRequestValidator : AbstractValidator<AdicionarFornecedorRequest>
    {
        private readonly string[] _ufsValidas = 
        {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", 
            "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", 
            "RS", "RO", "RR", "SC", "SP", "SE", "TO"
        };

        public AdicionarFornecedorRequestValidator()
        {
            RuleFor(request => request.InscricaoEstadual)
                .NotEmpty().WithMessage("A Inscrição Estadual é obrigatória.")
                .Length(8, 15).WithMessage("A Inscrição Estadual deve ter entre 8 e 15 caracteres.")
                .Matches(@"^[0-9]+$").WithMessage("A Inscrição Estadual deve conter apenas números.");

            RuleFor(request => request.NomeFantasia)
                .NotEmpty().WithMessage("O Nome Fantasia é obrigatório.")
                .Length(2, 100).WithMessage("O Nome Fantasia deve ter entre 2 e 100 caracteres.")
                .Matches(@"^[a-zA-ZÀ-ÿ0-9\s\-\.&]+$").WithMessage("O Nome Fantasia contém caracteres inválidos.");

            RuleFor(request => request.Uf)
                .NotEmpty().WithMessage("A UF é obrigatória.")
                .Length(2).WithMessage("A UF deve ter exatamente 2 caracteres.")
                .Must(BeValidUf).WithMessage("UF inválida.");

            RuleFor(request => request.Numero)
                .NotEmpty().WithMessage("O Número é obrigatório.")
                .Length(1, 10).WithMessage("O Número deve ter entre 1 e 10 caracteres.")
                .Matches(@"^[0-9A-Za-z\s\-]+$").WithMessage("Número do endereço inválido.");

            RuleFor(request => request.Bairro)
                .NotEmpty().WithMessage("O Bairro é obrigatório.")
                .Length(2, 50).WithMessage("O Bairro deve ter entre 2 e 50 caracteres.")
                .MustBeValidName();

            RuleFor(request => request.Cidade)
                .NotEmpty().WithMessage("A Cidade é obrigatória.")
                .Length(2, 50).WithMessage("A Cidade deve ter entre 2 e 50 caracteres.")
                .MustBeValidName();

            RuleFor(request => request.CepFornecedor.ToString())
                .MustBeValidCep()
                .When(request => request.CepFornecedor > 0);

            RuleFor(request => request.CepFornecedor)
                .GreaterThan(0).WithMessage("O CEP é obrigatório e deve ser maior que zero.")
                .InclusiveBetween(10000000, 99999999).WithMessage("CEP deve ter 8 dígitos.");

            RuleFor(request => request.StatusAtivo)
                .InclusiveBetween(0, 1).WithMessage("O Status Ativo deve ser 0 (inativo) ou 1 (ativo).");

            RuleFor(request => request.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .MustBeValidCnpj();

            RuleFor(request => request.Rua)
                .NotEmpty().WithMessage("A Rua é obrigatória.")
                .Length(5, 100).WithMessage("A Rua deve ter entre 5 e 100 caracteres.");

            RuleFor(request => request.Complemento)
                .Length(0, 50).WithMessage("O Complemento deve ter no máximo 50 caracteres.")
                .When(request => !string.IsNullOrEmpty(request.Complemento));
        }

        private bool BeValidUf(string uf)
        {
            return _ufsValidas.Contains(uf?.ToUpper());
        }
    }
}
