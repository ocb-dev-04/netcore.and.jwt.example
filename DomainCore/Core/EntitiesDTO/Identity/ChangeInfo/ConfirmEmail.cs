using System.ComponentModel.DataAnnotations;

namespace DomainCore.Core.EntitiesDTO.Identity.ChangeInfo
{
    public class ConfirmEmail
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string NewEmail { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
