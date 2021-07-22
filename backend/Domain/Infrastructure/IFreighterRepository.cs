using CblxChallenge.Domain.Entities;
using System.Threading.Tasks;

namespace CblxChallenge.Domain.Infrastructure
{
    public interface IFreighterRepository
    {
        Task AddCheckoutAsync(FreighterTransportEntity entity);

        Task AddCheckinAsync(FreighterTransportEntity entity);
    }
}
