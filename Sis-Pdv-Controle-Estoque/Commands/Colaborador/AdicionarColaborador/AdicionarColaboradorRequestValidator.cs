using FluentValidation;

namespace Commands.Colaborador.AdicionarColaborador
{
    public class AdicionarColaboradorRequestValidator : AbstractValidator<AdicionarColaboradorRequest>
    {
        public AdicionarColaboradorRequestValidator()
        {
            RuleFor(request => request.nomeColaborador).NotEmpty().WithMessage("O nome do colaborador é obrigatório.");

            RuleFor(request => request.DepartamentoId).NotEmpty().WithMessage("O ID do departamento é obrigatório.");

            RuleFor(request => request.cpfColaborador).NotEmpty().WithMessage("O CPF do colaborador é obrigatório.");
            RuleFor(request => request.cpfColaborador).Length(11).WithMessage("O CPF deve ter 11 dígitos.");

            RuleFor(request => request.cargoColaborador).NotEmpty().WithMessage("O cargo do colaborador é obrigatório.");

            RuleFor(request => request.telefoneColaborador).NotEmpty().WithMessage("O telefone do colaborador é obrigatório.");

            RuleFor(request => request.emailPessoalColaborador).NotEmpty().WithMessage("O e-mail pessoal do colaborador é obrigatório.");
            RuleFor(request => request.emailPessoalColaborador).EmailAddress().WithMessage("O e-mail pessoal do colaborador precisa ser um e-mail válido.");

            RuleFor(request => request.emailCorporativo).NotEmpty().WithMessage("O e-mail corporativo do colaborador é obrigatório.");
            RuleFor(request => request.emailCorporativo).EmailAddress().WithMessage("O e-mail corporativo do colaborador precisa ser um e-mail válido.");

            RuleFor(request => request.Usuario).NotNull().WithMessage("Usuário é obrigatório.");
        }
    }
}
