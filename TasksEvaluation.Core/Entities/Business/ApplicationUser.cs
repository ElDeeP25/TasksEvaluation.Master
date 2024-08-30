using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TasksEvaluation.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplictionUser class
public class ApplicationUser : IdentityUser
{
    [Required,MaxLength(255)]
    public string FirstName { get; set; }
    [Required, MaxLength(255)]
    public string LastName { get; set; }
   
    
    
    

}

