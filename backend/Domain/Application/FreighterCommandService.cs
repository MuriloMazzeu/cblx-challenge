using AutoMapper;
using CblxChallenge.Domain.Entities;
using CblxChallenge.Domain.Infrastructure;
using CblxChallenge.Domain.ViewModels;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
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
            return (availableTons * 1000) >= (total + (entity.Amount ?? 0));
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
       

        public async Task DataLoadAsync()
        {
            var period = string.Empty;
            var random = new Random();
            var date = DateTime.Now.Date.AddDays(-250);
            var dataMap = new Dictionary<string, string>()
            {
                { "I", "D" },
                { "II", "A" },
                { "III", "C" },
                { "IV", "B" },
            };

            Smk186Result availables = null;
            for (int i = 1; i <= 500; i++)
            {
                var now = date.AddDays(i);
                if(now.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }

                var newPeriod = GetWeeklyPeriod(now);
                if (newPeriod != period || availables is null)
                {
                    availables = await Smk186.GetMineralsAsync(newPeriod);
                    period = newPeriod;
                }

                foreach (var item in dataMap)
                {
                    var (tons, price) = item.Value switch
                    {
                        "A" => (availables.AMineralInTon, GetPriceByKilogram("A")),
                        "B" => (availables.BMineralInTon, GetPriceByKilogram("B")),
                        "C" => (availables.CMineralInTon, GetPriceByKilogram("C")),
                        "D" => (availables.DMineralInTon, GetPriceByKilogram("D")),
                        _ => throw new NotImplementedException(),
                    };

                    var (start, end) = GetDates(now, random);
                    var amount = Convert.ToInt32(tons * 1000);

                    var recommended = item.Value;
                    if (item.Key == "IV")
                    {
                        var bPrice = GetPriceByKilogram("B");
                        var cPrice = GetPriceByKilogram("C");
                        recommended = bPrice > cPrice ? "B" : "C";
                    }

                    await Repository.AddCheckoutAsync(new FreighterTransportEntity
                    {
                        Type = item.Key,
                        Amount = amount,
                        Mineral = item.Value,
                        Cost = amount * price,
                        EndAt = end,
                        StartAt = start,
                        Period = now.ToString("yyyy-MM"),
                        WeekPeriod = period,
                        RecommendedMineral = recommended
                    });
                }
            }
        }

        private (Timestamp start, Timestamp end) GetDates(DateTime date, Random random)
        {
            var start = date.ToUniversalTime()
                    .AddHours(random.Next(8, 24))
                    .AddMinutes(random.Next(1, 60))
                    .AddSeconds(random.Next(1, 60));

            var end = start
                .AddHours(random.Next(1, 24))
                .AddMinutes(random.Next(1, 60))
                .AddSeconds(random.Next(1, 60));

            return (Timestamp.FromDateTime(start), Timestamp.FromDateTime(end));
        }

        private string GetWeeklyPeriod(DateTime reference)
        {
            int week = 4, day = reference.Day;
            if (day >= 1 && day <= 7) week = 1;
            if (day >= 8 && day <= 14) week = 2;
            if (day >= 15 && day <= 21) week = 3;
            return $"{reference.Year}-{reference.Month}_{week}";
        }
    }
}
