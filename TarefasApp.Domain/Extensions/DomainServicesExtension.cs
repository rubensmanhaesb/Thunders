using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Domain.Entities;
using TarefasApp.Domain.Interfaces.Services;
using TarefasApp.Domain.Services;
using TarefasApp.Domain.Validations;

namespace TarefasApp.Domain.Extensions
{
    public static class DomainServicesExtension
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<ITarefaDomainService, TarefaDomainService>();
            services.AddTransient<IValidator<Tarefa>, TarefaValidator>();

            return services;
        }
    }
}
