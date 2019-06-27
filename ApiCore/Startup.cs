using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiCore.ConfigServices;
using DomainCore.Core.Interfaces.Identity;
using DomainCore.Core.Reps.Identity;
using DomainCore.Data.DataBaseContext;
using DomainCore.Data.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiCore
{
    public class Startup
    {
        #region Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Construct

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region ConfigureServices

        public void ConfigureServices(IServiceCollection services)
        {
            //  DI
            services.AddDI();
            //  DB
            services.AddDBInfo(Configuration);
            //  JWT
            services.AddJwtAuth(Configuration);
            //  Services (email, renderViews)
            services.AddApiServices(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //  config json result on mvc controllers
            services.AddMvc().AddJsonOptions(ConfigureJson);
        }

        //  just for configure json result 
        private void ConfigureJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        #endregion

        #region Configure

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc();
        }

        #endregion
    }
}
