using System.ComponentModel.DataAnnotations;

namespace DomainCore.Core.EntitiesDTO.Identity
{
    public class Login
    {
        #region Properties

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        #endregion
    }
}
