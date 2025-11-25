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
        private readonly ManageUserService _service;

        public manage_userController(ManageUserService service)
        {
            _service = service;
        }

        [HttpPost("add-user")]
        public async Task<APIResponseDTO<SuccessResponseDTO>> add(AddUserDTO user)
        {
            return await _service.AddUserService(user);
        }
    }
}
