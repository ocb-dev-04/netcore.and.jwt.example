using System.Threading.Tasks;
using DomainCore.Core.EntitiesDTO.Identity;

namespace DomainCore.Core.Interfaces.Identity
{
    public interface IIdentityUserRep
    {
        #region ConfirmMethods

        Task<bool> ConfirmEmailAsync(string email);

        #endregion

        #region GetById

        Task<AppIdentityUserDTO> GetById(string userId);

        #endregion

        #region Create/Update/Disable

        Task<AppIdentityUserDTO> CreateUserAsync(CreateAppIdentityUserDTO create);
        Task<AppIdentityUserDTO> CreatePartnerAsync(CreateAppIdentityUserDTO create);

        /*
         * this method is for create admin user, when you need create admin uncomment, 
         * when you make deploy delete this method
         * 
         * 
        Task<AppIdentityUserDTO> CreateAdminAsync(CreateAppIdentityUserDTO create);
        */

        Task<bool> DeleteUserAsync(string userId, string authUserId);

        #endregion

        #region Auth

        Task<JwtTokenDTO> AuthUserWebAsync(Login createToken);
        Task<JwtTokenDTO> AuthUserFixedAsync(Login createToken);
        Task LogoutAsync();

        #endregion
    }
}
