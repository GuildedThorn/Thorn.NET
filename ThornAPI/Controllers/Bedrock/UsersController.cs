using Microsoft.AspNetCore.Mvc;
using ThornData.Models.Bedrock;
using ThornData.Services.Bedrock;

namespace ThornAPI.Controllers.Bedrock; 

[ApiController]
[Route("/bedrock/[controller]")]
public class UsersController : ControllerBase {

    private readonly UserService _userService;

    public UsersController(UserService userService) {
        _userService = userService;
    }

    [HttpGet]
    public async Task<List<User>> GetAll() =>
        await _userService.GetAll();
    
    [HttpGet]
    [Route("{arg}")]
    public async Task<ActionResult<User>> Get(string arg) {
        var user = await _userService.GetUser(arg);

        if (user == null) {
            return NotFound();
        }
        
        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Create(User arg) {
        var user = await _userService.GetUser(arg.xuid);

        if (user is not null) {
            return NotFound();
        }

        await _userService.CreateUser(arg);
        return CreatedAtAction(nameof(Get), new { xuid = arg.xuid }, user);
    }
        
    
    [HttpPut]
    public async Task<IActionResult> Update(User arg) {
        var user = await _userService.GetUser(arg.xuid);

        if (user is null) {
            return NotFound();
        }

        await _userService.UpdateUser(arg);
        return Ok();
    }
}