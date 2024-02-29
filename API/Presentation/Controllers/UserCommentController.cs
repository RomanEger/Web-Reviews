using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    [Route("api/{videoId}/comments")]
    public class UserCommentController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public UserCommentController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
         
        [HttpGet]
        [ResponseCache(CacheProfileName = "5minutesDurationPrivate")]
        public async Task<IActionResult> GetUserComments(Guid videoId, [FromQuery] UserCommentsParameters commentsParameters)
        {
            var userComments = await _serviceManager.UserComment.GetUserCommentsAsync(videoId, commentsParameters, trackChanges: false);
            Response.Headers.Add("Info-Pagination", JsonSerializer.Serialize(userComments.metaData));
            return Ok(userComments.comments);
        }

        [Authorize]
        [HttpGet("{commentId}", Name ="UserCommentById")]
        public async Task<IActionResult> GetUserComment(Guid videoId, Guid commentId)
        {
            var userComment = await _serviceManager.UserComment.GetUserCommentById(videoId, commentId, trackChanges: false);
            return Ok(userComment);
        }

        [Authorize]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateUserComment(Guid videoId,[FromBody] UserCommentForManipulationDTO commentForManipulation)
        {
            var userComment = await _serviceManager.UserComment.CreateUserCommentAsync(videoId, commentForManipulation);
            return CreatedAtRoute("UserCommentById", new { videoId = videoId, commentId = userComment.UserCommentId }, userComment);
        }

        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteUserComment(Guid videoId, Guid commentId)
        {
            await _serviceManager.UserComment.DeleteUserCommentAsync(videoId, commentId, trackChanges: false);
            return NoContent();
        }

        [Authorize]
        [HttpPut("{commentId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateUserComment(Guid videoId, Guid commentId, [FromBody] UserCommentForManipulationDTO commentForManipulation)
        {
            var userComment = await _serviceManager.UserComment.UpdateUserCommentAsync(videoId, commentId, commentForManipulation, trackChanges: true);
            return Ok(userComment);
        }
    }
}
