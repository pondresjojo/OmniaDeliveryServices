using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.Infra.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageReceiver(this IServiceCollection services)
        {

            return services;
        }
    }
}
