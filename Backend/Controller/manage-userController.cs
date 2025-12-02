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
        public async Task<IActionResult> add(AddUserDTO user)
        {
            var result = await _service.AddUserService(user);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> get()
        {
            var result = await _service.GetUserService();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("patch-user/{id}")]
        public async Task<IActionResult> update(string id, UpdateUserDTO user)
        {
            var result = await _service.UpdateUserService(id, user);
            return StatusCode(result.StatusCode, result);
        }
        
        [HttpPatch("update-password/{id}")]
        public async Task<IActionResult> updatePassword(string id, PasswordDTO password)
        {
            var result = await _service.UpdatePasswordService(id, password);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> deleteUser(string id)
        {
            var result = await _service.DeleteUserService(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
