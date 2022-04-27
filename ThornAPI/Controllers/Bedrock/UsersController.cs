using Microsoft.AspNetCore.Mvc;
using ThornData.Models.Bedrock;
using ThornData.Services.Bedrock;

namespace ThornAPI.Controllers.Bedrock; 

[ApiController]
public class UserController : ControllerBase {

    private readonly UserService _userService;

    public UserController(UserService userService) {
        _userService = userService;
    }

    [HttpGet("GetAllUsers")]
    [Route("/bedrock/users/all")]
    public async Task<List<User>> GetAll() {
        return await _userService.GetAll();
    }

    [HttpGet("GetUserByIdOrName")]
    [Route("/bedrock/users/get/{arg}")]
    public async Task<User> Get(string arg) {
        if (arg.All(char.IsDigit)) {
            return await _userService.GetUserByXuid(arg);
        }
        return await _userService.GetUserByUsername(arg);
    }

    [HttpPost("CreateUser")]
    [Route("/bedrock/users/create")]
    public async Task Create(User user) =>
        await _userService.CreateUser(user);
    
    [HttpPut("UpdateUserByXuidOrName")]
    [Route("/bedrock/users/update/{arg}")]
    public async Task Update(string arg, User user) {
        if (arg.All(char.IsDigit)) {
            await _userService.UpdateUserByXuid(arg, user);
        }
        await _userService.UpdateUserByUsername(arg, user);
    }
}