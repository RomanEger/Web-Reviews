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
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public UserController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _serviceManager.User.GetUsersAsync(trackChanges: false);
            return Ok(users);
        }

        [HttpGet("{userId}", Name ="GetUserById")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var user = await _serviceManager.User.GetUserByIdAsync(userId, trackChanges: false);
            return Ok(user);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await _serviceManager.User.DeleteUserAsync(userId, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{userId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateUser([FromBody] UserForUpdateDTO userForUpdateDTO, Guid userId)
        {
            var user = await _serviceManager.User.UpdateUserAsync(userId, userForUpdateDTO, trackChanges: true);
            return Ok(user);
        }
    }
}
