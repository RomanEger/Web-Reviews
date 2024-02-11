using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/videostatuses")]
    public class VideoStatusController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public VideoStatusController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetVideoStatuses()
        {
            var videoStatuses = await _serviceManager.VideoStatuses.GetVideoStatusesAsync(trackChanges: false);
            return Ok(videoStatuses);
        }

        [HttpGet("{videoStatusId}", Name = "GetVideoStatusById")]
        public async Task<IActionResult> GetVideoStatus(Guid videoStatusId)
        {
            var videoStatus = await _serviceManager.VideoStatuses.GetVideoStatusByIdAsync(videoStatusId, trackChanges: false);
            return Ok(videoStatus);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateVideoStatus([FromBody] ReferenceForManipulationDTO manipulationDTO)
        {
            var videoStatus = await _serviceManager.VideoStatuses.CreateVideoStatusAsync(manipulationDTO);
            return CreatedAtRoute("GetVideoStatusById", new { videoStatusId = videoStatus.id }, videoStatus);
        }

        [HttpPut("{videoStatusId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateVideoStatus([FromBody] ReferenceForManipulationDTO manipulationDTO, Guid videoStatusId)
        {
            var videoStatus = await _serviceManager.VideoStatuses.UpdateVideoStatus(videoStatusId, manipulationDTO, trackChanges: true);
            return Ok(videoStatus);
        }

        [HttpDelete("{videoStatusId}")]
        public async Task<IActionResult> DeleteVideoStatus(Guid videoStatusId)
        {
            await _serviceManager.VideoStatuses.DeleteVideoStatusAsync(videoStatusId, trackChanges: false);
            return NoContent();
        }
    }
}
