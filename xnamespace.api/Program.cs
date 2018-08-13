using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;

namespace xnamespace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder().UseKestrel(options =>
            {
                options.Listen(IPAddress.Any, 443, listenOptions =>
                {
                    listenOptions.NoDelay = true;
                    listenOptions.UseConnectionLogging();
                });
            })
                .UseUrls("http://localhost:5000")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
