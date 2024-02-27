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
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthenticationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("registration")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDTO userForRegistration)
        {
            await _serviceManager.Authentication.CreateUserAsync(userForRegistration);
            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDTO userForAuthentication)
        {
            var userValid = await _serviceManager.Authentication.ValidateUser(userForAuthentication);
            if(!userValid)
                return Unauthorized();
            var tokens = await _serviceManager.Authentication.CreateToken(populateExp: true);
            return Ok(tokens);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByAccessToken([FromBody] TokenDTO tokenDTO)
        {
            var user = await _serviceManager.Authentication.GetUserByTokenAsync(tokenDTO);
            return Ok(user);
        }
    }
}
