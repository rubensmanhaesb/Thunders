using Microsoft.OpenApi.Models;
using System.Reflection;

namespace TarefasApp.API.Extensions
{
    /// <summary>
    /// Classe de extensão para configuração do Swagger (OPEN API)
    /// </summary>
    public static class SwaggerDocExtension
    {
        /// <summary>
        /// Método de extensão para configurar as preferências do Swagger
        /// </summary>
        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "TarefasApp API - Treinamento C# Avançado Formação Arquiteto",
                        Description = "API para controle de tarefas de usuários.",
                        Version = "1.0",
                        Contact = new OpenApiContact
                        {
                            Name = "COTI Informática",
                            Email = "contato@cotiinformatica.com.br",
                            Url = new Uri("http://wwww.cotiinformatica.com.br")
                        }
                    });

                    //configuração para incluir os comentários na documentação
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                });

            return services;
        }

        /// <summary>
        /// Método para configurar a execução do Swagger
        /// </summary>
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
