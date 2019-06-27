using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApiCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            CreateWebHostBuilder(args)
                .UseKestrel()
                .Build()
                .Run();
               
            /*
            try
            {
                var host = CreateWebHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var config = scope.ServiceProvider
                        .GetRequiredService<IConfiguration>();

                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(config)
                        .CreateLogger();


                }
                Log.Information("Starting web host");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
            */

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
