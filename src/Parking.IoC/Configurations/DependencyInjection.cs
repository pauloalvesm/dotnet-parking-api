using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parking.Data.Context;
using Parking.Data.Implementations;
using Parking.Domain.Interfaces.Repositories;
using Parking.Service.Implementations;
using Parking.Service.Interfaces;
using System.Reflection;

namespace Parking.IoC.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<ICustomerVehicleRepository, CustomerVehicleRepository>();
        services.AddScoped<IStayRepository, StayRepository>();

        var config = new TypeAdapterConfig();
        config.Scan(Assembly.Load("Parking.Service"));

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<ICustomerVehicleService, CustomerVehicleService>();
        services.AddScoped<IStayService, StayService>();

        return services;
    }
}
