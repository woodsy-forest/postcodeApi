using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Postcode.Constants;
using Postcode.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Postcode
{
    public class Program
    {
        static readonly HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            if (!ValidApiConnection())
            {
                Console.WriteLine("Cannot connect to the API.");
                return;
            }

            CreateDbIfNotExists(host);


            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<postcodeContext>();

                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static bool ValidApiConnection()
        {
            try
            {
                var url = ApiUrl.Postcode + "EX1 1NT";
                var res = client.GetAsync(url).Result;
                if (res.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ValidApiConnection, error: " + ex.ToString());
                return false;
            }


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
