using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainCore.Core.EntitiesDTO.Identity;
using DomainCore.Core.Interfaces.Identity;
using DomainCore.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiCore.Controllers.Identity
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        #region Properties

        private readonly IIdentityUserRep _identityUserRep;
        private readonly ILogger<AccountsController> _logger;

        #endregion

        #region Construct

        public AccountsController(
            IIdentityUserRep identityUserRep,
            ILogger<AccountsController> logger)
        {
            _identityUserRep = identityUserRep;
            _logger = logger;
        }

        #endregion

        #region Get's Methods

        [Route("user_id")]
        [HttpGet("{userId}", Name = "GetUserById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AppIdentityUserDTO>> GetById([FromRoute] string userId)
        {
            if (userId == "" || userId.Length < 10)
                return BadRequest("UserId is wrong");

            var response = await _identityUserRep.GetById(userId);
            if (response == null)
                return NotFound("User  not exist");

            return Ok(response);
        }

        #endregion

        #region Register

        #region Client

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateAppIdentityUserDTO create)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var confirm = await _identityUserRep.ConfirmEmailAsync(create.Email);
            if (!confirm)
                return BadRequest("Email is used");

            var result = await _identityUserRep.CreateUserAsync(create);
            if (result == null)
                return BadRequest("Some error ocurred while try create your account");

            return Ok(result);
        }

        #endregion

        #region Partner

        [Authorize(Roles = "Admin")]
        [Route("register_partner")]
        [HttpPost]
        public async Task<ActionResult> CreatePartner([FromBody] CreateAppIdentityUserDTO create)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var confirm = await _identityUserRep.ConfirmEmailAsync(create.Email);
            if (!confirm)
                return BadRequest("Email is used");

            var result = await _identityUserRep.CreatePartnerAsync(create);
            if (result == null)
                return BadRequest("Some error ocurred while try create your account");

            return Ok(result);
        }

        #endregion

        #region Admin

        /*
         * 
         * uncomment this just for create a admin user, when you mae deploy delete this code and 
         * code relations this in interfaces and reps, the rason is for nobody can make other admin user
         * 
         * 

        [Route("register_admin")]
        [HttpPost]
        public async Task<ActionResult> CreateAdmin([FromBody] CreateAppIdentityUserDTO create)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var confirm = await _identityUserRep.ConfirmEmailAsync(create.Email);
            if (!confirm)
                return BadRequest("Email is used");

            var result = await _identityUserRep.CreateAdminAsync(create);
            if (result == null)
                return BadRequest("Some error ocurred while try create your account");

            return Ok(result);
        }

        */

        #endregion

        #endregion

        #region Delete Methods

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("user_id")]
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser([FromRoute] string userId)
        {
            if (userId == "")
                return BadRequest("UserId can't be null");

            var authUserId = User.Identity.Name;
            var response = await _identityUserRep.DeleteUserAsync(userId, authUserId);
            if (!response)
                return NotFound("Some error ocurred while delete your account");

            return Ok("Account delete succesful");
        }

        
        #endregion
    }
}
