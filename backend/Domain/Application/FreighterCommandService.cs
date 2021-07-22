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
        public FreighterCommandService(IFreighterRepository repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        private IFreighterRepository Repository { get; }
        private IMapper Mapper { get; }

        private static double GetPriceByKilogram(string mineral)
        {
            return mineral.ToUpper() switch
            {
                "A" => 5d, // $ 5.000,00 / 10^3 kg
                "B" => 10_000_000d, // $ 10.000,00 / 10^-3 kg
                "C" => 30_000d, // $ 3.000,00 / 10^-1 kg
                "D" => 1d, // $100,00 / 10^2 kg
                _ => throw new NotSupportedException(),
            };
        }

        public async Task ExecuteAsync(FreighterCheckinCommand command)
        {
            var entity = Mapper.Map<FreighterTransportEntity>(command);
            var price = GetPriceByKilogram(command.Mineral);
            entity.CalculateCost(price);

            await Repository.AddCheckinAsync(entity);
        }

        public async Task ExecuteAsync(FreighterCheckoutCommand command)
        {
            var entity = Mapper.Map<FreighterTransportEntity>(command);
            await Repository.AddCheckoutAsync(entity);
        }
    }
}
