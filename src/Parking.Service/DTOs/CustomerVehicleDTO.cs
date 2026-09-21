using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Parking.Service.DTOs;

public class CustomerVehicleDTO
{
    [Range(0, int.MaxValue, ErrorMessage = "Invalid Id value")]
    public int Id { get; set; }

    [Required(ErrorMessage = "CustomerId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than zero")]
    public int? CustomerId { get; set; }

    [Required(ErrorMessage = "VehicleId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "VehicleId must be greater than zero")]
    public int? VehicleId { get; set; }

    [JsonIgnore]
    public CustomerDTO Customer { get; set; }

    [JsonIgnore]
    public VehicleDTO Vehicle { get; set; }
}