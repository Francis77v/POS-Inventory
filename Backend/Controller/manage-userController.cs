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
        public async Task<APIResponseDTO<SuccessResponseDTO>> add(AddUserDTO user, [FromServices] ManageUserService service)
        {
            var result = await service.AddUserService(user);
        }
    }
}
