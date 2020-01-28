using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoldenLadle.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GoldenLadle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://+:5000;https://+:5001");
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureKestrel(options => options.ListenAnyIP(Int32.Parse(Environment.GetEnvironmentVariable("PORT"))));
            });
    }
}