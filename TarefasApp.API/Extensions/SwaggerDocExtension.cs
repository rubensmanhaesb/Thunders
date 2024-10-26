using Microsoft.OpenApi.Models;
using System.Reflection;

namespace TarefasApp.API.Extensions
{
  
    public static class SwaggerDocExtension
    {

        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Thunders API - Desafio Técnico",
                        Description = "API para controle de tarefas de usuários.",
                        Version = "1.0",
                        Contact = new OpenApiContact
                        {
                            Name = "Rubens Bernardes",
                            Email = "rubensmanhaesb@hotmail.com",
                            Url = new Uri("https://www.thunders.com.br/")
                        }
                    });

                    //configuração para incluir os comentários na documentação
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TarefasApp");
            });

            return app;
        }
    }
}
