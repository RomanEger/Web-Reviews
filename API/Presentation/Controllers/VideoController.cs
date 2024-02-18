using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/videos")]
    public class VideoController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public VideoController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetVideos([FromBody] VideoParameters videoParameters)
        {
            var videos = await _serviceManager.Video.GetVideosAsync(videoParameters, trackChanges: false);
            return Ok(videos.videos);
        }
    }
}
