using CblxChallenge.Domain.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("data-load")]
    public class DataLoadController : ControllerBase
    {
        private ILogger<DataLoadController> Logger { get; }
        private IFreighterCommandService CommandService { get; }

        public DataLoadController(ILogger<DataLoadController> logger, IFreighterCommandService commandService)
        {
            Logger = logger;
            CommandService = commandService;
        }

        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                CommandService.DataLoadAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
