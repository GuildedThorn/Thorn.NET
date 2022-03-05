using System.ComponentModel.DataAnnotations;

namespace ThornData.Models; 

public class RegiserRequest {
    
    [Required]
    public string username { get; set; }
    
    [Required]
    public string first_name { get; set; }
    
    [Required]
    public string last_name { get; set; }
    
    public string email { get; set; }
    
    [Required]
    public string password { get; set; }
    
}