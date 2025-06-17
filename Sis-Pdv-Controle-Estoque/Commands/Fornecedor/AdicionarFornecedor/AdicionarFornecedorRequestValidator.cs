using Extensions;
using FluentValidation;

namespace Commands.Fornecedor.AdicionarFornecedor
{
    public class AdicionarFornecedorRequestValidator : AbstractValidator<AdicionarFornecedorRequest>
    {
        public AdicionarFornecedorRequestValidator()
        {
            RuleFor(request => request.inscricaoEstadual).NotEmpty().WithMessage("A Inscrição Estadual é obrigatória.");

            RuleFor(request => request.nomeFantasia).NotEmpty().WithMessage("O Nome Fantasia é obrigatório.");

            RuleFor(request => request.Uf).NotEmpty().WithMessage("A UF é obrigatória.");

            RuleFor(request => request.Numero).NotEmpty().WithMessage("O Número é obrigatório.");

            RuleFor(request => request.Bairro).NotEmpty().WithMessage("O Bairro é obrigatório.");

            RuleFor(request => request.Cidade).NotEmpty().WithMessage("A Cidade é obrigatória.");

            RuleFor(request => request.cepFornecedor).GreaterThan(0).WithMessage("O CEP é obrigatório e deve ser maior que zero.");

            RuleFor(request => request.statusAtivo).InclusiveBetween(0, 1).WithMessage("O Status Ativo deve ser 0 (inativo) ou 1 (ativo).");

            RuleFor(request => request.Cnpj).NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Must(BeAValidCNPJ).WithMessage("O CNPJ fornecido é inválido.");

            RuleFor(request => request.Rua).NotEmpty().WithMessage("A Rua é obrigatória.");
        }

        // Método de validação de CNPJ
        private bool BeAValidCNPJ(string cnpj)
        {
            return ValidaCPF.validarCpf(cnpj);
        }
    }
}
