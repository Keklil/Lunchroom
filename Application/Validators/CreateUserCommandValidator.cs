using Application.Commands;
using FluentValidation;

namespace Application.Validators;

public class CreateUserCommandValidator :
    AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.User.email).NotEmpty();
        RuleFor(x => x.User.password).NotEmpty();
    }
}