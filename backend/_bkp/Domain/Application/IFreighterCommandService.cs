using CblxChallenge.Domain.ViewModels;
using System.Threading.Tasks;

namespace CblxChallenge.Domain.Application
{
    public interface IFreighterCommandService
    {
        Task ExecuteAsync(FreighterCheckoutCommand command);
        Task<CommandResult> ExecuteAsync(FreighterCheckinCommand command);
    }
}
