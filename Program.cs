using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MyStarwarsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build(); 

            var host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureLogging(options => {
                    options.AddConsole(
                        LogLevel.Trace
                    );
                    options.AddDebug();
                })
                .UseConfiguration(configuration)
                .UseUrls("http://**:5000")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
