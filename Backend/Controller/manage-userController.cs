using Backend.DTOs.ManageUserDTO;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Services;
namespace Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class manage_userController : ControllerBase
    {
        [HttpPost("add-user")]
        public async Task<APIResponseDTO<SuccessResponseDTO>> add(AddUserDTO user, [FromServices] ManageUserService service)
        {
            return await service.AddUserService(user);
        }
    }
}
