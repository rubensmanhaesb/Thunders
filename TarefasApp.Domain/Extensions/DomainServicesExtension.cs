using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Domain.Interfaces.Services;
using TarefasApp.Domain.Services;

namespace TarefasApp.Domain.Extensions
{
    public static class DomainServicesExtension
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<ITarefaDomainService, TarefaDomainService>();

            return services;
        }
    }
}
