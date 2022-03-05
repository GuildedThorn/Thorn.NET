using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThornData.Models;

namespace ThornAPI.Controllers; 

[Authorize]
[ApiController]
[Route("/v1/users")]
public class UserController : ControllerBase {

    [AllowAnonymous]
    public IActionResult RegisterNewUser(RegiserRequest request) {
        return Ok(new { status = "200", message = "Succesfully registered user. "})
    }
    
    [HttpGet("GetUserByUsername")]
    [Route("/username/{username}")]
    public IActionResult GetUserByUsername(string username) {
        
    }

    [HttpGet("GetUserByUserId")]
    [Route("/uid/{uid}")]
    public IActionResult GetUserByUserId(string uid) {
        
    }

    
}