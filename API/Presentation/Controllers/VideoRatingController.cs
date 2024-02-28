using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Presentation.ActionFilters;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/videoratings")]
    public class VideoRatingController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public VideoRatingController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateOrUpdateVideoRating([FromBody] VideoRatingForManipulationDTO ratingForManipulation)
        {
            var video = await _serviceManager.VideoRating.CreateOrUpdateVideoRating(ratingForManipulation, trackChanges: true);
            return Ok(video);
        }
        
        [HttpGet("{videoId}")]
        public async Task<IActionResult> RefreshVideoRating(Guid videoId)
        {
            var video = await _serviceManager.VideoRating.RefreshVideoRating(videoId, trackChanges: true);
            return Ok(video);
        }
    }
}
