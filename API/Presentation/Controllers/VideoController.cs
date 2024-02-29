using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        [ResponseCache(CacheProfileName = "10minutesDurationPublic")]
        public async Task<IActionResult> GetVideos([FromQuery] VideoParameters videoParameters)
        {
            var videos = await _serviceManager.Video.GetVideosAsync(videoParameters, trackChanges: false);
            Response.Headers.Add("Info-Pagination", JsonSerializer.Serialize(videos.metaData));
            return Ok(videos.videos);
        }

        [HttpGet("{videoId}", Name ="GetVideoById")]
        public async Task<IActionResult> GetVideo(Guid videoId)
        {
            var video = await _serviceManager.Video.GetVideoByIdAsync(videoId, trackChanges: false);
            return Ok(video);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateVideo([FromBody] VideoForManipulationDTO videoForManipulation)
        {
            var createdVideo = await _serviceManager.Video.CreateVideoAsync(videoForManipulation);
            return CreatedAtRoute("GetVideoById", new { videoId = createdVideo.VideoId }, createdVideo);
        }

        [HttpDelete("{videoId}")]
        public async Task<IActionResult> DeleteVideo(Guid videoId)
        {
            await _serviceManager.Video.DeleteVideoAsync(videoId, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{videoId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateVideo([FromBody] VideoForManipulationDTO videoForManipulation, Guid videoId)
        {
            var video = await _serviceManager.Video.UpdateVideoAsync(videoId, videoForManipulation, trackChanges: true);
            return Ok(video);
        }
    }
}
