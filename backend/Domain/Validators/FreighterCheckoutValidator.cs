using CblxChallenge.Domain.ViewModels;
using FluentValidation;
using System;

namespace CblxChallenge.Domain.Validators
{
    public class FreighterCheckoutValidator : AbstractValidator<FreighterCheckoutCommand>
    {
        public FreighterCheckoutValidator()
        {
            RuleFor(x => x.RequestId)
                .NotEmpty();

            RuleFor(x => x.StartAt)
                .LessThan(DateTime.MaxValue)
                .GreaterThan(DateTime.MinValue)
                .Must(v => v.TimeOfDay.Hours >= 8)
                .WithMessage("O horário mínimo de partida é 8:00");

            RuleFor(x => x.Type)
                .Matches("^(I|II|III|IV)$")
                .WithMessage("O tipo de classe está inválido");
        }
    }
}
