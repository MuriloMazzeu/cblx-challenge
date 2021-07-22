using CblxChallenge.Domain.ViewModels;
using System.Threading.Tasks;

namespace CblxChallenge.Domain.Infrastructure
{
    public interface ISmk186Service
    {
        Task<Smk186Result> GetMineralsAsync(string period);
    }
}
