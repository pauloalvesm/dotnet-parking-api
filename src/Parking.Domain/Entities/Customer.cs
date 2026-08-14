using Parking.Domain.Common;
using Parking.Domain.Validations;

namespace Parking.Domain.Entities;

public class Customer : Entity
{
    public string Name { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public string Cpf { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public int AddressId { get; private set; }

    public Address Address { get; private set; }
    public ICollection<CustomerVehicle> CustomerVehicles { get; private set; } = new List<CustomerVehicle>();

    public Customer(int id, string name, DateOnly? birthDate, string cpf, string phone, string email, int addressId)
    {
        ValidateDomain(name, birthDate, cpf, phone, email, addressId);

        Id = id;
        Name = name;
        BirthDate = birthDate;
        Cpf = cpf;
        Phone = phone;
        Email = email;
        AddressId = addressId;
    }

    private void ValidateDomain(string name, DateOnly? birthDate, string cpf, string phone, string email, int addressId)
    {
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(name), "Name is required");
        DomainExceptionValidation.GetErrors(name.Length > 100, "Name cannot exceed 100 characters");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(cpf), "CPF is required");
        DomainExceptionValidation.GetErrors(cpf.Length != 11, "CPF must be 11 characters long");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(phone), "Phone is required");
        DomainExceptionValidation.GetErrors(phone.Length > 15, "Phone cannot exceed 15 characters");
        DomainExceptionValidation.GetErrors(string.IsNullOrWhiteSpace(email), "Email is required");
        DomainExceptionValidation.GetErrors(email.Length > 100, "Email cannot exceed 100 characters");

        if (birthDate.HasValue)
        {
            string birthDateString = birthDate.Value.ToString("dd/MM/yyyy");
            DomainExceptionValidation.GetErrors(!DateTime.TryParseExact(birthDateString, "dd/MM/yyyy", null,
                System.Globalization.DateTimeStyles.None, out _), "BirthDate must be in the format DD/MM/YYYY");
        }

        DomainExceptionValidation.GetErrors(addressId <= 0, "AddressId is required and must be greater than zero");
    }

}
