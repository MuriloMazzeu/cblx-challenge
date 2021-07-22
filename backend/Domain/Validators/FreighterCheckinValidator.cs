using CblxChallenge.Domain.ViewModels;
using FluentValidation;
using System;

namespace CblxChallenge.Domain.Validators
{
    public class FreighterCheckinValidator : AbstractValidator<FreighterCheckinCommand>
    {
        public FreighterCheckinValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.RequestId)
                .NotEmpty();

            RuleFor(x => x.Amount)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.EndAt)
                .LessThan(DateTime.MaxValue)
                .GreaterThan(DateTime.MinValue);

            RuleFor(x => x.Mineral)
                .Matches("^(A|B|C|D)$")
                .WithMessage("O tipo de mineral está inválido");
        }
    }
}
