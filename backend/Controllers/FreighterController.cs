using CblxChallenge.Domain.Application;
using CblxChallenge.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CblxChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FreighterController : ControllerBase
    {
        private ILogger<FreighterController> Logger { get; }
        private IFreighterCommandService CommandService { get; }

        public FreighterController(ILogger<FreighterController> logger, IFreighterCommandService commandService)
        {
            Logger = logger;
            CommandService = commandService;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckinAsync(FreighterCheckinCommand checkin)
        {
            try
            {
                return Ok(new CommandResult
                {
                    Success = false,
                    Message = "Teste de Erro"
                });

                await CommandService.ExecuteAsync(checkin);
                return Ok(new CommandResult
                {
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RequestId = " + checkin.RequestId);
                return BadRequest(new CommandResult
                {
                    Success = false,
                });
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutAsync(FreighterCheckoutCommand checkout)
        {
            try
            {
                await CommandService.ExecuteAsync(checkout);
                return Ok(new CommandResult
                {
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RequestId = " + checkout.RequestId);
                return BadRequest(new CommandResult
                {
                    Success = false,
                });
            }
        }
    }
}
