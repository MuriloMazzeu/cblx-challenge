using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace CblxChallenge
{
    public static class Program
    {
        public static void Main(string[] args) => 
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var path = AppContext.BaseDirectory + "firebase-service-account.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(wb =>
            {
                wb.UseStartup<Startup>();
            });
        }
    }
}
