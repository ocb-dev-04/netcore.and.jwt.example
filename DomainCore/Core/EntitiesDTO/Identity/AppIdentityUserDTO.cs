using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainCore.Core.EntitiesDTO.Identity
{
    public class AppIdentityUserDTO
    {
        #region Properties

        //  id take  from JWT but not is required because user just have JWT not userId explicit
        public string Id { get; set; }

        public string FullName { get; set; }

        //  just need email because password 
        public string Email { get; set; }
        
        public string Role { get; set; }

        #endregion
    }
}
