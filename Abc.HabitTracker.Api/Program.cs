using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abc.HabitTracker.Api.Tracker.Constants;
using Abc.HabitTracker.Api.Tracker.Database;
using Abc.HabitTracker.Api.Tracker.Habit;
using Abc.HabitTracker.Api.Tracker.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Abc.HabitTracker.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Scheduler.Run();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
