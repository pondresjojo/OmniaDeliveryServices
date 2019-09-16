﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OmniaDelivery.Data;
using OmniaDelivery.Infra.Implementations;
using OmniaDelivery.Infra.Interfaces;
using OmniaDelivery.InputProcessingService.Services;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OmniaDelivery.InputProcessingService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }


        private static IHostBuilder CreateHostBuilder(string[] args)
        {



            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddJsonFile($"appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddEnvironmentVariables("DOTNET_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {

                    var sqlConnectionString = hostContext.Configuration.GetConnectionString("DeliveryGatewayCN");
                    services.AddDbContext<DeliveryContext>(options => options.UseSqlServer(sqlConnectionString));

                    services.AddTransient<IMessageHandler>((svc) =>
                    {
                        var rabbitMQConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                        string rabbitMQHost = rabbitMQConfigSection["Host"];
                        string rabbitMQUserName = rabbitMQConfigSection["UserName"];
                        string rabbitMQPassword = rabbitMQConfigSection["Password"];
                        return new RabbitMQConsumer(rabbitMQHost, rabbitMQUserName, rabbitMQPassword, "delivery.files.direct", "files.delivered", ""); ;
                    });

                    services.AddTransient<IMessagePublisher>((svc) =>
                    {
                        var rabbitConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                        string host = rabbitConfigSection["Host"];
                        string user = rabbitConfigSection["UserName"];
                        string password = rabbitConfigSection["Password"];
                        return new RabbitMQMessagePublisher(host, user, password, "Omnia");
                    });


                    services.AddTransient<ISingleService, SingleService>();
                    services.AddHostedService<ProcessingManager>();

                    //services.ConfigureOptions<AppSettings>();
                    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    services.AddHttpClient<IDeliveryService, DeliveryService>().SetHandlerLifetime(TimeSpan.FromMinutes(5));

                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .UseConsoleLifetime();

            return hostBuilder;
        }

    }
}
