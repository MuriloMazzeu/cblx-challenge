using AutoMapper;
using CblxChallenge.Domain.Entities;
using CblxChallenge.Domain.Infrastructure;
using CblxChallenge.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CblxChallenge.Domain.Application
{
    public class ReceivedMineralsQueryService : IReceivedMineralsQueryService
    {
        public ReceivedMineralsQueryService(IReceivedMineralsRepository repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        private IReceivedMineralsRepository Repository { get; }
        private IMapper Mapper { get; }

        private static double CalculateBusyTime(FreighterTransportEntity entity)
        {
            if (!entity.EndAt.HasValue) throw new InvalidOperationException();
            var endAt = entity.EndAt.Value.ToDateTime();
            var startAt = entity.StartAt.ToDateTime();

            var diff = endAt - startAt;
            return diff.TotalMinutes;
        }

        private static int CalculateIdlenessIndex(double total, double busy)
        {
            var idleness = Math.Abs(total - busy) / total;
            return Convert.ToInt32(Math.Round(idleness * 100d));
        }

        public async Task<ReceivedMinerals> ExecuteAsync(ReceivedMineralsQuery query)
        {
            var period = query.Period.ToString("yyyy-MM");
            var items = await Repository.GetByPeriodAsync(period);
            var data = Mapper.Map<IEnumerable<ReceivedMineralsData>>(items);

            var costs = items.GroupBy(i => i.Mineral)
                .Select(i => new { i.Key, Total = i.Sum(s => s.Cost ?? 0d) })
                .ToDictionary(i => i.Key, i => i.Total);

            var effectiveStart = new DateTime(query.Period.Year, query.Period.Month, 1);
            var effectiveEnd = effectiveStart.AddMonths(1).AddDays(-1);
            var total = (effectiveEnd - effectiveStart).TotalMinutes;

            var index = items.GroupBy(i => i.Type)
                .Select(i => new { i.Key, Busy = i.Sum(CalculateBusyTime) })
                .ToDictionary(i => i.Key, i => CalculateIdlenessIndex(total, i.Busy));

            return new ReceivedMinerals
            {
                Items = data,
                TotalCost = costs,
                IdlenessIndex = index
            };
        }
    }
}
