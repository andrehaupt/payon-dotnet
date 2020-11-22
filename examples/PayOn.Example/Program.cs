using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PayOn.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Settings.EntityId_3DSecure = "xxx";
            Settings.EntityId_Recurring = "xxx";
            Settings.UserId = "xxx";
            Settings.Password = "xxx";
            Settings.BaseUrl = "https://test.oppwa.com";

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
