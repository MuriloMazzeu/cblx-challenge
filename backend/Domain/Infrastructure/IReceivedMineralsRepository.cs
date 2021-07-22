using CblxChallenge.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CblxChallenge.Domain.Infrastructure
{
    public interface IReceivedMineralsRepository
    {
        Task<IEnumerable<FreighterTransportEntity>> GetByPeriodAsync(string period);
    }
}
