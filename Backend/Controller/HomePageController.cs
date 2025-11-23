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
        public async Task<IActionResult> login(LoginDTO user, [FromServices] AuthServices service)
        {
            try
            {
                var result = await service.CheckUserService(user);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new APIResponseDTO<object>()
                {
                    success = false,
                    StatusCode = 500,
                    message = "Internal Error",
                    Errors = new List<string>()
                    {
                        e.Message
                    }
                });
            }
            
        }
    }
}
