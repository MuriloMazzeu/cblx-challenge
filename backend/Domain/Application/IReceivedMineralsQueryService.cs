using CblxChallenge.Domain.ViewModels;
using System.Threading.Tasks;

namespace CblxChallenge.Domain.Application
{
    public interface IReceivedMineralsQueryService
    {
        Task<ReceivedMinerals> ExecuteAsync(ReceivedMineralsQuery query);
    }
}
