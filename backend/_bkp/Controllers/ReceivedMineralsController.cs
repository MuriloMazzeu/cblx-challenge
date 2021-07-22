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
        private readonly ILogger<ReceivedMineralsController> _logger;
        private readonly IReceivedMineralsQueryService _queryService;

        public ReceivedMineralsController(ILogger<ReceivedMineralsController> logger, IReceivedMineralsQueryService queryService)
        {
            _logger = logger;
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ReceivedMineralsQuery query)
        {
            try
            {
                var result = await _queryService.ExecuteAsync(query);
                return Ok(new QueryResult<ReceivedMinerals>
                {
                    Success = true,
                    Data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RequestId = " + query.RequestId);
                return BadRequest(new QueryResult<ReceivedMinerals>
                {
                    Success = false,
                });
            }
        }
    }
}
