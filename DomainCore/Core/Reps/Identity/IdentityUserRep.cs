using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using DomainCore.Data.Entities.Identity;
using DomainCore.Core.EntitiesDTO.Identity;
using DomainCore.Core.Interfaces.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace DomainCore.Core.Reps.Identity
{
    public class IdentityUserRep : IIdentityUserRep
    {
        #region Properties

        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        #endregion

        #region Construct

        public IdentityUserRep(
            UserManager<AppIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppIdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        #endregion

        #region ConfirmMethods

        public async Task<bool> ConfirmEmailAsync(string email)
        {
            var confirm = await _userManager.FindByEmailAsync(email);
            if (confirm != null)
                return false;

            return true;
        }

        #endregion

        #region Get's Methods

        public async Task<AppIdentityUserDTO> GetById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            #region Convert to DTO

            var userInfo = new AppIdentityUserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };

            #endregion

            return userInfo;
        }

        #endregion

        #region CRUDMethods

        #region Create

        #region Client

        public async Task<AppIdentityUserDTO> CreateUserAsync(CreateAppIdentityUserDTO create)
        {
            //  take the appsetting value to role and asign to a var
            string role = _configuration["UserRoles:Client"];

            #region Convert to Identity class

            var user = new AppIdentityUser
            {
                FullName = create.FullName,
                Email = create.Email,
                UserName = create.Email,
                PhoneNumber = create.Phone,
                //  add default role
                Role = role
            };

            #endregion

            var result = await _userManager.CreateAsync(user, create.Password);
            if (!result.Succeeded)
                throw new ArgumentNullException(nameof(result));

            //  find the user to add role and return the id
            var userCreated = await _userManager.FindByEmailAsync(create.Email);
            //  add user to IdentityUserRole
            var addUserToRole = await _userManager.AddToRoleAsync(userCreated, role);

            #region Convert to DTO

            var userInfo = new AppIdentityUserDTO
            {
                Id = userCreated.Id,
                FullName = userCreated.UserName,
                Email = create.Email,
                Role = role
            };

            #endregion

            return userInfo;
        }

        #endregion

        #region Partner

        public async Task<AppIdentityUserDTO> CreatePartnerAsync(CreateAppIdentityUserDTO create)
        {
            //  take the appsetting value to role and asign to a var
            string role = _configuration["UserRoles:Partner"];
            //  if user send some role, change the object value to null, cero, nothing
            create.RoleName = "";

            #region Convert to Identity class

            var user = new AppIdentityUser
            {
                FullName = create.FullName,
                Email = create.Email,
                UserName = create.Email,
                PhoneNumber = create.Phone,
                //  add default role
                Role = role
            };

            #endregion

            var result = await _userManager.CreateAsync(user, create.Password);
            if (!result.Succeeded)
                throw new ArgumentNullException(nameof(result));

            //  find the user to add role and return the id
            var userCreated = await _userManager.FindByEmailAsync(create.Email);
            //  add user to IdentityUserRole
            var addUserToRole = await _userManager.AddToRoleAsync(userCreated, role);

            #region Convert to DTO

            var userInfo = new AppIdentityUserDTO
            {
                Id = userCreated.Id,
                FullName = userCreated.UserName,
                Email = create.Email,
                Role = role
            };

            #endregion

            return userInfo;
        }

        #endregion

        #region Admin

        /*
         * this is repository for create admin user, uncomment just for create admin, and when you
         * make deploy delete this method
         * 
         * 

        public async Task<AppIdentityUserDTO> CreateAdminAsync(CreateAppIdentityUserDTO create)
        {
            //  take the appsetting value to role and asign to a var
            string role = "SuperAdminMasterOwner";
            //  if user send some role, change the object value to null, cero, nothing
            create.RoleName = "";

            #region Convert to Identity class

            var user = new AppIdentityUser
            {
                FullName = create.FullName,
                Email = create.Email,
                UserName = create.Email,
                PhoneNumber = create.Phone,
                //  add default role
                Role = role
            };

            #endregion

            var result = await _userManager.CreateAsync(user, create.Password);
            if (!result.Succeeded)
                throw new ArgumentNullException(nameof(result));

            //  find the user to add role and return the id
            var userCreated = await _userManager.FindByEmailAsync(create.Email);

            //  create admin role, when the role of admin is created this line can by comment
            await _roleManager.CreateAsync(new IdentityRole(role));
            //  add user to IdentityUserRole
            var addUserToRole = await _userManager.AddToRoleAsync(userCreated, role);

            #region Convert to DTO

            var userInfo = new AppIdentityUserDTO
            {
                Id = userCreated.Id,
                FullName = userCreated.UserName,
                Email = create.Email,
                Role = role
            };

            #endregion

            return userInfo;
        }

        */

        #endregion

        #endregion

        #region Update's



        #endregion

        #region Delete

        public async Task<bool> DeleteUserAsync(string userId, string authUserId)
        {
            //  confirm if userId that wanna delete is the same of authUserId
            if (!userId.Equals(authUserId))
                return false;

            //  find the user with this Id
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            //  confirm if the user try delete is'nt admin 
            if (user.Role == "SuperAdminMasterOwner")
                return false;

            //  remove this client on stripe and subsccription for this client for now just is  true
            var removeFromPay = true; //await _paymentRep.RemoveClientAsync(userId);

            //  delete the user totally
            var deleteUser = await _userManager.DeleteAsync(user);

            //  confirm all is ok, if not....return false
            if (!removeFromPay || deleteUser == null)
                return false;

            return true;
        }

        #endregion

        #endregion

        #region Auth Methods

        //  for web auth
        public async Task<JwtTokenDTO> AuthUserWebAsync(Login createToken)
        {
            var user = await _userManager.FindByEmailAsync(createToken.Email);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = await _signInManager.PasswordSignInAsync(user, createToken.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
                throw new ArgumentNullException(nameof(result));

            #region CreateToken

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Site"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
               Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["Jwt:ExpiryTimeWeb"])),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            #endregion

            var tokenCreated = new JwtTokenDTO
            {
                Token = tokenHandler.WriteToken(token)
            };

            return tokenCreated;
        }

        //  for fixed auth
        public async Task<JwtTokenDTO> AuthUserFixedAsync(Login createToken)
        {
            var user = await _userManager.FindByEmailAsync(createToken.Email);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = await _signInManager.PasswordSignInAsync(user, createToken.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
                throw new ArgumentNullException(nameof(result));

            #region CreateToken

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Site"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)

                }),
                Expires = DateTime.UtcNow.AddYears(Convert.ToInt32(_configuration["Jwt:ExpiryTimeFixed"])),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            #endregion

            var tokenCreated = new JwtTokenDTO
            {
                Token = tokenHandler.WriteToken(token)
            };

            return tokenCreated;
        }

        //  for logout
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion
    }
}
