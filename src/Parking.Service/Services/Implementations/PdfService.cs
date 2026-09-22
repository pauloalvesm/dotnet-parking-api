using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Logging;
using Parking.Service.DTOs;
using Parking.Service.Exceptions;
using Parking.Service.Interfaces;
using System.Globalization;

namespace Parking.Service.Services.Implementations;

public class PdfService : IPdfService
{
    private readonly ILogger<PdfService> _logger;

    public PdfService(ILogger<PdfService> logger)
    {
        _logger = logger;
    }

    public byte[] GenerateStayPdf(StayDTO stayDto)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            var writer = new PdfWriter(memoryStream);
            var pdfDocument = new PdfDocument(writer);
            var document = new Document(pdfDocument);

            document.Add(new Paragraph("Vehicle Parking Receipt")
                .SetBold()
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.BLUE));

            var table = new Table(UnitValue.CreatePercentArray([1, 2]))
                .SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Description")).SetBold().SetFontColor(ColorConstants.BLUE));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Value")).SetBold().SetFontColor(ColorConstants.BLUE));

            table.AddCell("Customer Vehicle ID:");
            table.AddCell(stayDto.CustomerVehicleId?.ToString() ?? "N/A");

            table.AddCell("License Plate:");
            table.AddCell(stayDto.LicensePlate ?? "N/A");

            table.AddCell("Entry Date:");
            table.AddCell(stayDto.EntryDate.HasValue
                ? stayDto.EntryDate.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
                : "N/A");

            table.AddCell("Exit Date:");
            table.AddCell(stayDto.ExitDate.HasValue
                ? stayDto.ExitDate.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
                : "N/A");

            table.AddCell("Hourly Rate:");
            table.AddCell(stayDto.HourlyRate.ToString("C", CultureInfo.CurrentCulture));

            table.AddCell("Total Amount:");
            table.AddCell(stayDto.TotalAmount.HasValue
                ? stayDto.TotalAmount.Value.ToString("C", CultureInfo.CurrentCulture)
                : "N/A");

            table.AddCell("Status:");
            table.AddCell(stayDto.StayStatus.ToString());

            document.Add(table);

            document.Add(new Paragraph("Developed by - Paulo Alves")
                .SetBold()
                .SetFontSize(8)
                .SetTextAlignment(TextAlignment.RIGHT));

            document.Close();
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF for Stay ID: {Id}", stayDto?.Id);
            throw new PdfStayException("An error occurred while generating the PDF.", ex);
        }
    }
}