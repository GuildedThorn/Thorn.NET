using Microsoft.AspNetCore.Mvc;

namespace ThornAPI.Controllers; 

[ApiController]
[Route("v1/users")]
public class UsersController : ControllerBase {

    [HttpGet]
    [Route("/{username}")]
    public Array GetUser(string username) {
        
    }
    
    
}