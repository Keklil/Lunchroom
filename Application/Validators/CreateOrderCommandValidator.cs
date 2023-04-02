using Application.Commands;
using FluentValidation;

namespace Application.Validators;

public class CreateOrderCommandValidator :
    AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.CustomerId).NotEmpty();

        RuleFor(x => x.Order.MenuId).NotEmpty();

        RuleFor(x => x.Order.Options)
            .ForEach(options => options
                .ChildRules(option =>
                {
                    option.RuleFor(o => o.OptionId).NotEmpty();
                    option.RuleFor(o => o.Units).NotNull();
                })
                .Must(option => option.Units > 0).WithMessage("Number of option units is not valid"));
    }
}