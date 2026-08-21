using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace Parking.IoC.Configurations;

public static class DependencyInjectionSwagger
{
    public static IServiceCollection AddInfrastructureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Parking",
                Version = "v1",
                Description = "API to simulate a parking management system.",
                Contact = new OpenApiContact
                {
                    Name = "Paulo Alves",
                    Email = "paulo.alves7351@gmail.com",
                    Url = new Uri("https://github.com/pauloalvesm"),
                },
            });
        });

        return services;
    }
}
