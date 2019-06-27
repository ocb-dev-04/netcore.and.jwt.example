using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainCore.Core.EntitiesDTO.Identity
{
    public class UpdateAppIdentityUserDTO
    {
        #region Properties

        //  id take  from JWT but not is required because user just have JWT not userId explicit
        public string Id { get; set; }
        //  just need email because password 
        [Required]
        public string Email { get; set; }
        //  if the user wanna change something

        [Required]
        public string Password { get; set; }

        #endregion
    }
}
