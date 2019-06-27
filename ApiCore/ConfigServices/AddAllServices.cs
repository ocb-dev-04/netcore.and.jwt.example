using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DomainCore.Core.Reps.Identity;
using DomainCore.Data.DataBaseContext;
using DomainCore.Data.Entities.Identity;
using DomainCore.Core.Interfaces.Identity;
using DomainCore.Core.Interfaces;
using DomainCore.Core.Reps.App;

namespace ApiCore.ConfigServices
{
    public static class AddAllServices
    {
        #region AddDI

        //  add all dependecies inyeccion
        public static void AddDI(this IServiceCollection services)
        {
            //  DI for identity (CRUD Users)
            services.AddScoped<IIdentityUserRep, IdentityUserRep>();

            //  app di in self
            services.AddScoped<IExampleRep, ExampleRep>();
        }

        #endregion

        #region DBConnect

        public static void AddDBInfo(
            this IServiceCollection services,
            IConfiguration Configuration)
        {
            //  config context to DataBase
            services.AddDbContext<AppDbContext>(
                option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //  identity config (to create users)
            services.AddIdentity<AppIdentityUser, IdentityRole>(
                    option =>
                    {
                        //  for email confirmation
                        option.User.RequireUniqueEmail = true;

                        //  password config
                        option.Password.RequireDigit = true;
                        option.Password.RequiredLength = 6;
                        option.Password.RequireNonAlphanumeric = false;
                        option.Password.RequireUppercase = false;
                        option.Password.RequireLowercase = true;
                    }
                ).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }

        #endregion

        #region AddJWTAuth

        public static void AddJwtAuth(
            this IServiceCollection services,
            IConfiguration Configuration)
        {
            //  jwt auth config
            services.
                AddAuthentication(option => {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //  uncomment this if you wanna use just a website
                        ValidateIssuer = true,
                        ValidIssuer = Configuration.GetValue<string>("Jwt:Site"),

                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

            #endregion
    }
}
