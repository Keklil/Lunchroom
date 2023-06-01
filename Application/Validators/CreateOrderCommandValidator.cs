using Application.Commands;
using FluentValidation;

namespace Application.Validators;

public class CreateOrderCommandValidator :
    AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.MenuId).NotEmpty();
    }
}