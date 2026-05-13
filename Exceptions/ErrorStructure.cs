public class ErrorStructure
{
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public ErrorStructure(int statusCode, string message, string? details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }
}