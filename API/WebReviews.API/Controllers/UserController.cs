using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebReviews.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public async Task<IActionResult> CreateUser(User user)
        {
           return Created("uri",user);
        }

        public async Task<IActionResult> GetUser(Guid id)
        {
            return Ok();
        }

        public async Task<IActionResult> UpdateUser(Guid id,User user)
        {
            return Ok();
        }

        public async Task<IActionResult> DeleteUser(Guid id)
        {
            return NoContent();
        }
        
    }
}