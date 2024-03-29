﻿using System.ComponentModel.DataAnnotations;

namespace ThornData.Models; 

public class UserLogin {
    
    [Required(ErrorMessage = "Username is required")]
    public string? username { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? password { get; set; }
    
}