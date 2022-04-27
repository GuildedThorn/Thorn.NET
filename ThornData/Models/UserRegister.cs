using System.ComponentModel.DataAnnotations;

namespace ThornData.Models; 

public class UserRegister {

    [Required(ErrorMessage = "Username is required")]
    public string? username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? password { get; set; }
    
    [Required(ErrorMessage = "First Name is required")]
    public string? FirstName { get; set; }
    
    [Required(ErrorMessage = "Last Name is required")]
    public string? LastName { get; set; }
    
}