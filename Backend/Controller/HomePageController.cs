using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs.Auth;
using Backend.Services.Authentication;
using Backend.DTOs;
namespace Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO user, [FromServices] AuthServices service)
        {
            var result = await service.CheckUserService(user);
            return StatusCode(result.StatusCode, result);
        }

    }
}
