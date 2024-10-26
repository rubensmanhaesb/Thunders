using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Domain.Interfaces.Repositories;
using TarefasApp.Infra.Data.Contexts;
using TarefasApp.Infra.Data.Repositories; 

namespace TarefasApp.Infra.Data.Extensions
{
    public static class DataContextExtension
    {
        public static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>
                (options => options.UseSqlServer(configuration.GetConnectionString("BDTarefasApp")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
