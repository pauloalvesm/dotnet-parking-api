using System.ComponentModel.DataAnnotations;

namespace Parking.Service.DTOs;

public class CustomerDTO
{
    [Range(0, int.MaxValue, ErrorMessage = "Invalid Id value")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; }

    public DateOnly? BirthDate { get; set; }

    [Required(ErrorMessage = "CPF is required")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF must be 11 characters long")]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "Phone is required")]
    [StringLength(15, ErrorMessage = "Phone cannot exceed 15 characters")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; }

    [Required(ErrorMessage = "AddressId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "AddressId must be greater than zero")]
    public int AddressId { get; set; }

    public AddressDTO Address { get; set; }
}