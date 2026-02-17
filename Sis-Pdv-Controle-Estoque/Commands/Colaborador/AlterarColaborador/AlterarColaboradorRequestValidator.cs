using FluentValidation;

namespace Commands.Colaborador.AlterarColaborador
{
    public class AlterarColaboradorRequestValidator : AbstractValidator<AlterarColaboradorRequest>
    {
        public AlterarColaboradorRequestValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("O ID é obrigatório.");

            RuleFor(request => request.NomeColaborador).NotEmpty().WithMessage("O nome do colaborador é obrigatório.");

            RuleFor(request => request.DepartamentoId).NotEmpty().WithMessage("O ID do departamento é obrigatório.");

            RuleFor(request => request.CpfColaborador).NotEmpty().WithMessage("O CPF do colaborador é obrigatório.");
            RuleFor(request => request.CpfColaborador).Length(11).WithMessage("O CPF deve ter 11 dígitos.");

            RuleFor(request => request.CargoColaborador).NotEmpty().WithMessage("O cargo do colaborador é obrigatório.");

            RuleFor(request => request.TelefoneColaborador).NotEmpty().WithMessage("O telefone do colaborador é obrigatório.");

            RuleFor(request => request.EmailPessoalColaborador).NotEmpty().WithMessage("O e-mail pessoal do colaborador é obrigatório.");
            RuleFor(request => request.EmailPessoalColaborador).EmailAddress().WithMessage("O e-mail pessoal do colaborador precisa ser um e-mail válido.");

            RuleFor(request => request.EmailCorporativo).NotEmpty().WithMessage("O e-mail corporativo do colaborador é obrigatório.");
            RuleFor(request => request.EmailCorporativo).EmailAddress().WithMessage("O e-mail corporativo do colaborador precisa ser um e-mail válido.");

            RuleFor(request => request.Usuario).NotNull().WithMessage("Usuário é obrigatório.");
        }
    }
}
