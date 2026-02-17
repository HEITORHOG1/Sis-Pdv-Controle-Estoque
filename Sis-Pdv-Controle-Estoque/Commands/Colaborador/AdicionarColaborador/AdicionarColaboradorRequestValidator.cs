using FluentValidation;
using Validators;

namespace Commands.Colaborador.AdicionarColaborador
{
    public class AdicionarColaboradorRequestValidator : AbstractValidator<AdicionarColaboradorRequest>
    {
        public AdicionarColaboradorRequestValidator()
        {
            RuleFor(request => request.NomeColaborador)
                .NotEmpty().WithMessage("O nome do colaborador é obrigatório.")
                .Length(2, 100).WithMessage("O nome do colaborador deve ter entre 2 e 100 caracteres.")
                .MustBeValidName();

            RuleFor(request => request.DepartamentoId)
                .NotEmpty().WithMessage("O ID do departamento é obrigatório.");

            RuleFor(request => request.CpfColaborador)
                .NotEmpty().WithMessage("O CPF do colaborador é obrigatório.")
                .MustBeValidCpf();

            RuleFor(request => request.CargoColaborador)
                .NotEmpty().WithMessage("O cargo do colaborador é obrigatório.")
                .Length(2, 50).WithMessage("O cargo deve ter entre 2 e 50 caracteres.");

            RuleFor(request => request.TelefoneColaborador)
                .NotEmpty().WithMessage("O telefone do colaborador é obrigatório.")
                .MustBeValidBrazilianPhone();

            RuleFor(request => request.EmailPessoalColaborador)
                .NotEmpty().WithMessage("O e-mail pessoal do colaborador é obrigatório.")
                .MustBeValidEmail();

            RuleFor(request => request.EmailCorporativo)
                .NotEmpty().WithMessage("O e-mail corporativo do colaborador é obrigatório.")
                .MustBeValidEmail();

            RuleFor(request => request.Usuario)
                .NotNull().WithMessage("Usuário é obrigatório.");

            RuleFor(request => request.Id)
                .NotEqual(Guid.Empty).WithMessage("ID do colaborador inválido.")
                .When(request => request.Id != Guid.Empty);

            // Cross-field validation: emails should be different
            RuleFor(request => request.EmailCorporativo)
                .NotEqual(request => request.EmailPessoalColaborador)
                .WithMessage("E-mail corporativo deve ser diferente do e-mail pessoal.")
                .When(request => !string.IsNullOrWhiteSpace(request.EmailPessoalColaborador) && 
                                !string.IsNullOrWhiteSpace(request.EmailCorporativo));
        }
    }
}
