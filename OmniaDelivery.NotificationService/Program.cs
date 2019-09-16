using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OmniaDelivery.Infra.Implementations;
using OmniaDelivery.Infra.Interfaces;
using OmniaDelivery.NotificationService.Implementations;
using OmniaDelivery.NotificationService.Models;
using OmniaDelivery.NotificationService.NotificationChannels;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OmniaDelivery.NotificationService
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

                    var sqlConnectionString = hostContext.Configuration.GetConnectionString("NotificationCN");
                    services.AddDbContext<NotificationContext>(options => options.UseSqlServer(sqlConnectionString));

                    services.AddTransient<IMessageHandler>((svc) =>
                    {
                        var rabbitMQConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                        string rabbitMQHost = rabbitMQConfigSection["Host"];
                        string rabbitMQUserName = rabbitMQConfigSection["UserName"];
                        string rabbitMQPassword = rabbitMQConfigSection["Password"];
                        return new RabbitMQConsumer(rabbitMQHost, rabbitMQUserName, rabbitMQPassword, "delivery.workflow.fanout", "delivery.notification", "");
                    });


                    //services.AddTransient<IMessageHandler>((svc) =>
                    //{
                    //    var azureConfigSection = hostContext.Configuration.GetSection("AzureQueue");
                    //    string azureEndpoint = azureConfigSection["Endpoint"];
                    //    string azureQueuename = azureConfigSection["Queuename"];
                    //    return new AzureQueueConsumer(azureEndpoint, azureQueuename);
                    //});

                    //services.AddTransient<IMessagePublisher>((svc) =>
                    //{
                    //    var rabbitConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                    //    string host = rabbitConfigSection["Host"];
                    //    string user = rabbitConfigSection["UserName"];
                    //    string password = rabbitConfigSection["Password"];
                    //    return new RabbitMQMessagePublisher(host, user, password, "delivery.kla0102.direct");
                    //});


                    //services.AddHostedService<ConsumerManager>();
                    services.AddTransient<IEmailNotifier>((svc) =>
                    {
                        var mailConfigSection = hostContext.Configuration.GetSection("Email");
                        string mailHost = mailConfigSection["Host"];
                        int mailPort = Convert.ToInt32(mailConfigSection["Port"]);
                        string mailUserName = mailConfigSection["User"];
                        string mailPassword = mailConfigSection["Pwd"];
                        return new SMTPEmailNotifier(mailHost, mailPort, mailUserName, mailPassword);
                    });

                    services.AddTransient<IAzureQueueNotifier>((svc) =>
                    {
                        var mailConfigSection = hostContext.Configuration.GetSection("AzureQueue");
                        string endPoiny = mailConfigSection["EndPoint"];
                        return new AzureQueueNotifier(endPoiny);
                    });

                    services.AddHostedService<NotificationManager>();


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
