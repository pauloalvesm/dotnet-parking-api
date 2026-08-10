using Parking.Domain.Common;
using Parking.Domain.Validations;

namespace Parking.Domain.Entities;

public class Address : Entity
{
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string FederativeUnit { get; private set; }
    public string City { get; private set; }
    public string ZipCode { get; private set; }

    public ICollection<Customer> Customers { get; private set; } = new List<Customer>();

    public Address(int id,
                   string street,
                   string number,
                   string complement,
                   string neighborhood,
                   string federativeUnit,
                   string city,
                   string zipCode)
    {
        ValidateDomain(street, number, complement, neighborhood, federativeUnit, city, zipCode);

        Id = id;
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        FederativeUnit = federativeUnit;
        City = city;
        ZipCode = zipCode;
    }

    private void ValidateDomain(string street,
                               string number,
                               string complement,
                               string neighborhood,
                               string federativeUnit,
                               string city,
                               string zipCode)
    {
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(street), "Street is required");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(number), "Number is required");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(neighborhood), "Neighborhood is required");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(federativeUnit), "FederativeUnit is required");
        DomainExceptionValidation.GetErrors(federativeUnit.Length != 2, "FederativeUnit must be 2 characters long");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(city), "City is required");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(zipCode), "ZipCode is required");
        DomainExceptionValidation.GetErrors(zipCode.Length != 9, "ZipCode must be 9 characters long");

        if (complement != null)
        {
            DomainExceptionValidation.GetErrors(complement.Length > 150, "Complement cannot exceed 150 characters");
        }
    }
}
