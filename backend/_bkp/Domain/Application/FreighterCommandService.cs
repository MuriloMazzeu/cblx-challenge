using AutoMapper;
using CblxChallenge.Domain.Entities;
using CblxChallenge.Domain.Infrastructure;
using CblxChallenge.Domain.ViewModels;
using System;
using System.Threading.Tasks;

namespace CblxChallenge.Domain.Application
{
    public class FreighterCommandService : IFreighterCommandService
    {
        public FreighterCommandService(IFreighterRepository repository, IMapper mapper, ISmk186Service smk186)
        {
            Repository = repository;
            Mapper = mapper;
            Smk186 = smk186;
        }

        private IFreighterRepository Repository { get; }
        private IMapper Mapper { get; }
        private ISmk186Service Smk186 { get; }

        private static double GetPriceByKilogram(string mineral)
        {
            return mineral switch
            {
                "A" => 5d, // $ 5.000,00 / 10^3 kg
                "B" => 10_000_000d, // $ 10.000,00 / 10^-3 kg
                "C" => 30_000d, // $ 3.000,00 / 10^-1 kg
                "D" => 1d, // $100,00 / 10^2 kg
                _ => throw new NotSupportedException(),
            };
        }

        private async Task<bool> CheckAmountAsync(FreighterTransportEntity entity)
        {
            var mineral = entity.Mineral;
            var period = entity.WeekPeriod;

            var availables = await Smk186.GetMineralsAsync(period);
            var availableTons = mineral switch
            {
                "A" => availables.AMineralInTon,
                "B" => availables.BMineralInTon,
                "C" => availables.CMineralInTon,
                "D" => availables.DMineralInTon,
                _ => throw new NotSupportedException(),
            };

            var total = await Repository.GetTotalKilogramsInWeekAsync(period, mineral);
            return (availableTons * 1000) > (total + (entity.Amount ?? 0));
        }

        public async Task<CommandResult> ExecuteAsync(FreighterCheckinCommand command)
        {
            var mineral = command.Mineral;
            var price = GetPriceByKilogram(mineral);

            var entity = Mapper.Map<FreighterTransportEntity>(command);
            entity.SetWeeklyPeriod(command.StartAt);
            entity.CalculateCost(price);

            var isValid = await CheckAmountAsync(entity);
            if (isValid)
            {
                await Repository.AddCheckinAsync(entity);
                return new CommandResult
                {
                    Success = true
                };
            }
            else
            {
                return new CommandResult
                {
                    Success = false,
                    Message = "Quantidade de minério maior que a disponível"
                };
            }
        }

        public async Task ExecuteAsync(FreighterCheckoutCommand command)
        {
            var entity = Mapper.Map<FreighterTransportEntity>(command);
            if (entity.Type == "IV")
            {
                var bPrice = GetPriceByKilogram("B");
                var cPrice = GetPriceByKilogram("C");
                entity.RecommendedMineral = bPrice > cPrice ? "B" : "C";
            }

            await Repository.AddCheckoutAsync(entity);
        }
    }
}
