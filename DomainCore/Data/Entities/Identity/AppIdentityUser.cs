using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainCore.Data.Entities.Identity
{
    public class AppIdentityUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        
        [Required]
        public string Role { get; set; }
    }
}
