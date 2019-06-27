using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DomainCore.Core.EntitiesDTO.Identity;
using DomainCore.Core.Interfaces.Identity;

namespace ApiCore.Controllers.Identity
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        #region Properties

        private readonly IIdentityUserRep _identityUserRep;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        #endregion

        #region Construct

        public AuthController(
            IIdentityUserRep identityUserRep,
            ILogger<AuthController> logger,
            IConfiguration configuration)
        {
            _identityUserRep = identityUserRep;
            _logger = logger;
            _configuration = configuration;
        }

        #endregion

        #region Login and Token

        [Route("auth_user")] // login from website
        [HttpPost]
        public async Task<ActionResult> LoginAndToken([FromBody] Login createToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var auth = await _identityUserRep.AuthUserWebAsync(createToken);
            if (auth == null)
                return NotFound("Some error ocurred while try create the token");

            return Ok(new
            {
                token = auth.Token
            });
        }

        [Route("auth_user_fixed")] // login from desktop or app mobile
        [HttpPost]
        public async Task<ActionResult> LoginAndTokenFixed([FromBody] Login createToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var auth = await _identityUserRep.AuthUserFixedAsync(createToken);
            if (auth == null)
                return NotFound("Some error ocurred while try create the token");

            return Ok(new
            {
                token = auth.Token
            });
        }

        #endregion

        #region LogOut

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("logout")] // logout from anywhere
        [HttpPost]
        public async Task<ActionResult> LogOut()
        {
            //  first delete JWT

            //  then logout
            await _identityUserRep.LogoutAsync();

            return Ok("You logout");
        }

        #endregion
    }
}
