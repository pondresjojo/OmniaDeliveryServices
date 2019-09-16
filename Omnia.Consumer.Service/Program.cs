using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OmniaDelivery.Infra.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using OmniaDelivery.ConsumerService;
using OmniaDelivery.Infra.Implementations;

namespace Omnia.Consumer.Service
{
    public class Program
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
                    services.AddTransient<IMessageHandler>((svc) =>
                    {
                        var azureConfigSection = hostContext.Configuration.GetSection("AzureQueue");
                        string azureEndpoint = azureConfigSection["Endpoint"];
                        string azureQueuename = azureConfigSection["Queuename"];
                        return new AzureQueueConsumer(azureEndpoint, azureQueuename); 
                    });

                    services.AddTransient<IMessagePublisher>((svc) =>
                    {
                        var rabbitConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                        string host = rabbitConfigSection["Host"];
                        string user = rabbitConfigSection["UserName"];
                        string password = rabbitConfigSection["Password"];
                        return new RabbitMQMessagePublisher(host, user, password, "delivery.kla0102.direct",2);
                    });


                    services.AddHostedService<ConsumerManager>();
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
