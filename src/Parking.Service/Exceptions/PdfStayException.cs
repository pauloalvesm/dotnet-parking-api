namespace Parking.Service.Exceptions;

public class PdfStayException : Exception
{
    public PdfStayException(string message, Exception innerException) : base(message, innerException) { }
}