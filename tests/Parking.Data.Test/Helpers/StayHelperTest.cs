using Microsoft.EntityFrameworkCore;
using Parking.Data.Context;
using Parking.Domain.Entities;
using Parking.Domain.Enums;

namespace Parking.Data.Test.Helpers;

public static class StayTestHelper
{
    public static ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    public static ApplicationDbContext GetFaultyDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=invalid_host;Database=invalid_db;Username=invalid;Password=invalid;Timeout=1")
            .Options;

        return new ApplicationDbContext(options);
    }

    public static Stay CreateValidStay(int id = 1)
    {
        var customerVehicle = (CustomerVehicle)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(CustomerVehicle));
        var customer = (Customer)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(Customer));
        var vehicle = (Vehicle)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(Vehicle));

        PopulateRequiredStringProperties(customer);
        PopulateRequiredStringProperties(vehicle);

        SetPropertyIfExists(customer, "Id", 1);
        SetPropertyIfExists(customer, "AddressId", 1);
        SetPropertyIfExists(customer, "Cpf", "12345678901");
        SetPropertyIfExists(customer, "Phone", "11999999999");
        SetPropertyIfExists(customer, "Email", "test@test.com");

        SetPropertyIfExists(vehicle, "Id", 1);

        typeof(CustomerVehicle).GetProperty(nameof(CustomerVehicle.Customer))?.SetValue(customerVehicle, customer);
        typeof(CustomerVehicle).GetProperty(nameof(CustomerVehicle.Vehicle))?.SetValue(customerVehicle, vehicle);

        return new Stay(
            id: id,
            customerVehicleId: 1,
            licensePlate: "ABC-1234",
            entryDate: DateTime.UtcNow.AddHours(-2),
            exitDate: null,
            hourlyRate: 10.0m,
            totalAmount: null,
            stayStatus: StayStatus.Parked,
            customerVehicle: customerVehicle
        );
    }

    private static void PopulateRequiredStringProperties(object obj)
    {
        var properties = obj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        foreach (var prop in properties)
        {
            if (prop.PropertyType == typeof(string) && prop.CanWrite && prop.GetValue(obj) == null)
            {
                prop.SetValue(obj, "TestValue");
            }
        }
    }

    private static void SetPropertyIfExists(object obj, string propertyName, object value)
    {
        var prop = obj.GetType().GetProperty(propertyName);
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(obj, value);
        }
    }
}