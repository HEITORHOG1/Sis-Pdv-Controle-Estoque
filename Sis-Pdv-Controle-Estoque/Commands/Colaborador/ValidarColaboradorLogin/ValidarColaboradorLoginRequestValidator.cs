using FluentValidation;
using Sis_Pdv_Controle_Estoque.Commands.Colaborador.ValidarColaboradorLogin;

namespace ValidarColaboradorLogin
{
    public class ValidarColaboradorLoginRequestValidator : AbstractValidator<ValidarColaboradorLoginRequest>
    {
        public ValidarColaboradorLoginRequestValidator()
        {
            RuleFor(request => request.Login).NotEmpty().WithMessage("O Login é obrigatório.");

            RuleFor(request => request.Senha).NotEmpty().WithMessage("A Senha é obrigatória.");
            RuleFor(request => request.Senha).MinimumLength(6).WithMessage("A Senha deve conter pelo menos 6 caracteres.");
        }
    }
}
