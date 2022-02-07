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
    
    private static readonly string[] Summaries = new[] {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    [HttpPost("CreateUser")]
    [Route("/create/{username}")]
    public void CreateUser(string username, string password, string email) {
    }
    
    [HttpDelete("DeleteUser")]
    [Route("/delete/{username}")]
    public void DeleteUser(string username, string token) {
        
    }
}