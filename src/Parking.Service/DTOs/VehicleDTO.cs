using System.ComponentModel.DataAnnotations;
using Parking.Domain.Enums;

namespace Parking.Service.DTOs;

public class VehicleDTO
{
    [Range(0, int.MaxValue, ErrorMessage = "Invalid Id value")]
    public int Id { get; set; }

    [Required(ErrorMessage = "VehicleType is required")]
    [EnumDataType(typeof(VehicleType), ErrorMessage = "Invalid VehicleType")]
    public VehicleType VehicleType { get; set; }

    [Required(ErrorMessage = "Brand is required")]
    [StringLength(50, ErrorMessage = "Brand cannot exceed 50 characters")]
    public string Brand { get; set; }

    [Required(ErrorMessage = "Model is required")]
    [StringLength(50, ErrorMessage = "Model cannot exceed 50 characters")]
    public string Model { get; set; }

    [Required(ErrorMessage = "Color is required")]
    [StringLength(50, ErrorMessage = "Color cannot exceed 50 characters")]
    public string Color { get; set; }

    [Range(1900, 2100, ErrorMessage = "VehicleYear must be between 1900 and 2100")]
    public int? VehicleYear { get; set; }

    [StringLength(200, ErrorMessage = "Notes cannot exceed 200 characters")]
    public string Notes { get; set; }
}