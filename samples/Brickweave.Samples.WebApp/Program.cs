using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Brickweave.Samples.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost
                .CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false)
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
