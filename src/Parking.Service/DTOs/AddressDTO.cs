using System.ComponentModel.DataAnnotations;

namespace Parking.Service.DTOs;

public class AddressDTO
{
    [Range(0, int.MaxValue, ErrorMessage = "Invalid Id value")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Street is required")]
    [StringLength(150, ErrorMessage = "Street cannot exceed 150 characters")]
    public string Street { get; set; }

    [Required(ErrorMessage = "Number is required")]
    [StringLength(20, ErrorMessage = "Number cannot exceed 20 characters")]
    public string Number { get; set; }

    [StringLength(150, ErrorMessage = "Complement cannot exceed 150 characters")]
    public string Complement { get; set; }

    [Required(ErrorMessage = "Neighborhood is required")]
    [StringLength(100, ErrorMessage = "Neighborhood cannot exceed 100 characters")]
    public string Neighborhood { get; set; }

    [Required(ErrorMessage = "FederativeUnit is required")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "FederativeUnit must be 2 characters long")]
    public string FederativeUnit { get; set; }

    [Required(ErrorMessage = "City is required")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string City { get; set; }

    [Required(ErrorMessage = "ZipCode is required")]
    [StringLength(9, MinimumLength = 9, ErrorMessage = "ZipCode must be 9 characters long")]
    public string ZipCode { get; set; }
}