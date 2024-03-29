﻿using Microsoft.AspNetCore.Mvc;
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
    [Route("api/tokens")]
    public class TokenController :ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TokenController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDTO tokenDTO)
        {
            var newTokens = await _serviceManager.Authentication.RefreshToken(tokenDTO);
            return Ok(newTokens);
        }

        [HttpGet("decode")]
        public async Task<IActionResult> GetUserByAccessToken([FromQuery] string accessToken)
        {
            var user = await _serviceManager.Authentication.GetUserByTokenAsync(accessToken);
            return Ok(user);
        }
    }
}
