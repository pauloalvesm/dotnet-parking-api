using System.ComponentModel.DataAnnotations;
using Parking.Domain.Enums;

namespace Parking.Service.DTOs;

public class StayDTO
{
    [Range(0, int.MaxValue, ErrorMessage = "Invalid Id value")]
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "CustomerVehicleId must be greater than zero")]
    public int? CustomerVehicleId { get; set; }

    [Required(ErrorMessage = "LicensePlate is required")]
    [StringLength(10, ErrorMessage = "LicensePlate cannot exceed 10 characters")]
    public string LicensePlate { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? ExitDate { get; set; }

    [Required(ErrorMessage = "HourlyRate is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "HourlyRate must be greater than zero")]
    public decimal HourlyRate { get; set; }

    public decimal? TotalAmount { get; set; }

    [Required(ErrorMessage = "StayStatus is required")]
    [EnumDataType(typeof(StayStatus), ErrorMessage = "Invalid StayStatus")]
    public StayStatus StayStatus { get; set; }

    public CustomerVehicleDTO CustomerVehicle { get; set; }
}