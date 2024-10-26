using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Infra.Storage.Contexts;
using TarefasApp.Infra.Storage.Persistence;
using TarefasApp.Infra.Storage.Settings;

namespace TarefasApp.Infra.Storage.Extensions
{
    public static class MongoDBExtension
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {           
            var mongodbSettings = new MongoDBSettings();

            //ler as configurações do /appsettings.json
            new ConfigureFromConfigurationOptions<MongoDBSettings>
                (configuration.GetSection("MongoDB"))
                .Configure(mongodbSettings);

            //registrando as configurações
            services.AddSingleton(mongodbSettings);

            services.AddSingleton<MongoDBContext>();
            services.AddTransient<TarefaPersistence>();

            return services;
        }
    }
}
