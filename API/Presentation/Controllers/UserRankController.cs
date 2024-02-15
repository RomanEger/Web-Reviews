using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/userranks")]
    public class UserRankController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public UserRankController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRanks()
        {
            var userRanksToReturn = await _serviceManager.UserRank.GetUserRanksAsync(trackChanges: false);
            return Ok(userRanksToReturn);
        }

        [HttpGet("{userRankId}")]
        public async Task<IActionResult> GetUserRank(Guid userRankId)
        {
            var userRankToReturn = await _serviceManager.UserRank.GetUserRankAsync(userRankId, trackChanges: false);
            return Ok(userRankToReturn);
        }
    }
}
