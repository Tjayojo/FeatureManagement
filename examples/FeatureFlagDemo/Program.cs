﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace FeatureFlagDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) => config.AddJsonFile(
                    @"C:\appsettings\pan-feature-management\machineconfig.json",
                    false,
                    true))
                .UseStartup<Startup>();
    }
}