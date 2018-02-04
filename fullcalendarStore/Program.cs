using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using fullcalendarStore.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace fullcalendarStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webhost = BuildWebHost(args);
            var scope = webhost.Services.CreateScope();

            // Quick & dirty workaround waiting for database Docker container
            System.Threading.Thread.Sleep(5000);

            using (var applicationDb = scope.ServiceProvider.GetService<ApplicationDbContext>())
            {
                applicationDb.Database.Migrate();
            }

            using (var fullcalendarStore = scope.ServiceProvider.GetService<FullcalendarStoreContext>())
            {
                fullcalendarStore.Database.Migrate();
            }

            webhost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
