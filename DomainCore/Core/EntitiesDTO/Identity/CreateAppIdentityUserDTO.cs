using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DomainCore.Core.EntitiesDTO.Identity
{
    public class CreateAppIdentityUserDTO
    {
        #region Properties

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]

        public string Phone { get; set; }

        public string RoleName { get; set; }

        #endregion
    }
}
