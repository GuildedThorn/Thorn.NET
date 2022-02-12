using Microsoft.AspNetCore.Mvc;

namespace ThornAPI.Controllers; 

[ApiController]
[Route("v1/users")]
public class UsersController : ControllerBase {

    [HttpGet()]
    [Route("/{username}")]
    public string[] GetUser(string username) {
        return Summaries;
    }
    
    
    [HttpPost("CreateUser")]
    [Route("/create/{username}&password={password}&email={email}")]
    public void CreateUser(string username, string password, string email) {
    }
    
    [HttpDelete("DeleteUser")]
    [Route("/delete/{username}")]
    public void DeleteUser(string username, string token) {
        
    }
}