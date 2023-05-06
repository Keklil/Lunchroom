using Application.Commands;
using Application.Commands.Menu;
using FluentValidation;

namespace Application.Validators;

public class CreateMenuCommandValidator :
    AbstractValidator<CreateMenuCommand>
{
    public CreateMenuCommandValidator()
    {
        RuleFor(x => x.Menu.LunchSets)
            .ForEach(lunchSets => lunchSets
                .ChildRules(lunchSet =>
                {
                    lunchSet.RuleFor(ls => ls.LunchSetList)
                        .ForEach(lunchSetUnit => lunchSetUnit.NotEmpty());
                })
                .Must(lunchSet => lunchSet.Price > 0).WithMessage("Lunch set price is not valid"));

        RuleFor(x => x.Menu.Options)
            .ForEach(options => options
                .ChildRules(option =>
                {
                    option.RuleFor(o => o.Name).NotEmpty();
                    option.RuleFor(o => o.Price).NotEmpty();
                })
                .Must(option => option.Price > 0).WithMessage("Option price is not valid"));
    }
}