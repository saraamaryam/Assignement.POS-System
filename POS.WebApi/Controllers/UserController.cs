using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace POS.API.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
  
    public class UserController : ControllerBase
    {
        [HttpGet("user-info")]
        public IActionResult GetUserInfo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not authenticated.");
            }

            var userClaims = User.Claims;

            var userInfo = new
            {
                UserId = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                UserName = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                UserRole = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
        };

            return Ok(userInfo);
        }
    }

}
