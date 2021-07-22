using CblxChallenge.Domain.Application;
using CblxChallenge.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CblxChallenge.Controllers
{
    [ApiController]
    [Route("received-minerals")]
    public class ReceivedMineralsController : ControllerBase
    {
        private ILogger<ReceivedMineralsController> Logger { get; }
        private IReceivedMineralsQueryService QueryService { get; }

        public ReceivedMineralsController(ILogger<ReceivedMineralsController> logger, IReceivedMineralsQueryService queryService)
        {
            Logger = logger;
            QueryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ReceivedMineralsQuery query)
        {
            try
            {
                var result = await QueryService.ExecuteAsync(query);
                return Ok(new QueryResult<ReceivedMinerals>
                {
                    Success = true,
                    Data = result,
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RequestId = " + query.RequestId);
                return BadRequest(new QueryResult<ReceivedMinerals>
                {
                    Success = false,
                });
            }
        }
    }
}
